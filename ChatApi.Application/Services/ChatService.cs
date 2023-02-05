using ChatApi.Application.Extensions;
using ChatApi.Application.Models;
using ChatApi.Application.Services.Interfaces;
using ChatApi.Domain.DTOs;
using ChatApi.Domain.Entities;
using ChatApi.Persistence.Data;
using ChatApi.WebSocket.Handlers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ChatApi.Application.Services
{
    public class ChatService : IChatService
    {
        private readonly RoomHandler _ws;
        private readonly AppDbContext _db;

        public ChatService(RoomHandler ws, AppDbContext db)
        {
            _ws = ws;
            _db = db;
        }

        public async Task<ResponseModel> Visualize(string userId, Guid? chatId)
        {
            try
            {
               var messages = await _db.Messages.Where(x => x.ChatId == chatId && x.SenderUserId != userId).ToListAsync();

                foreach(var message in messages)
                {
                    message.Visualized = true;
                }
                  
                _db.UpdateRange(messages);
                await _db.SaveChangesAsync();
                
                string wsResponse = JsonConvert.SerializeObject(new { chatId = chatId }, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                await _ws.SendMessageToGroup(userId, wsResponse);

                return new ResponseModel(200, "Mensagens atualizadas");
            }
            catch
            {
                return new ResponseModel(500, "Ocorreu um erro!");
            }
        }

        public async Task<ResponseModel> GetUserChats(string userId)
        {
            try
            {
                var chats = await _db.Chats.Where(x => x.SenderUserId == userId || x.RecipientUserId == userId).ToListAsync();
                return ResponseModel.BuildOkResponse("Operação realizada com sucesso!", chats);
            }
            catch
            {
                return new ResponseModel(500, "Ocorreu um erro!");
            }
        }

        public async Task<ResponseModel> GetUserPaginatedChats(ChatFilterModel model)
        {
            try
            {
                var chats = (await _db.Chats.ApplyFilter(model).Include(x => x.Messages).ToListAsync()).Select(x => new ChatReducedDTO(x.Id, x.RecipientUserId == model.UserId ? x.SenderUser! : x.RecipientUser!, x.Messages!.OrderByDescending(x => x.CreatedAt).FirstOrDefault()!, x.Messages!.Count(x => x.SenderUserId != model.UserId && !x.Visualized)));
                return new ResponseModel(200, chats);
            }
            catch
            {
                return new ResponseModel(500, "Ocorreu um erro!");
            }
        }

        public async Task<ResponseModel> GetUserMessages(Guid chatId, int page)
        {
            try
            {
                var messages = await _db.Messages.OrderByDescending(x => x.CreatedAt).ApplyFilter(chatId).ReturnPaginated(page);
                return new ResponseModel(200, messages);
            }
            catch
            {
                return new ResponseModel(500, "Ocorreu um erro!");
            }
        }

        public async Task<ResponseModel> Add(AddChatModel model)
        {
            try
            {
                Chat? chat = await _db.Chats.FirstOrDefaultAsync(x => (x.SenderUserId == model.SenderUserId && x.RecipientUserId == model.RecipientUserId) || (x.SenderUserId == model.RecipientUserId && x.RecipientUserId == model.SenderUserId));

                if(chat != null)
                {
                    return ResponseModel.BuildOkResponse("Operação realizada com sucesso!", new { chatId = chat.Id });
                }

                var result = await _db.AddAsync(new Chat
                {
                    SenderUserId = model.SenderUserId,
                    RecipientUserId = model.RecipientUserId,
                });
                
                await _db.SaveChangesAsync();

                chat = result.Entity;

                var senderUser = await _db.Users.FirstOrDefaultAsync(x => x.Id == model.SenderUserId);

                string wsResponse = JsonConvert.SerializeObject(new { chatId = chat.Id, chat = new ChatReducedDTO(chat.Id, senderUser!, null), isAdding = true }, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

                await _ws.SendMessageToGroup(model.RecipientUserId, wsResponse);
                return ResponseModel.BuildOkResponse("Operação realizada com sucesso!", new { chatId = chat.Id });
            }
            catch
            {
                return new ResponseModel(500, "Ocorreu um erro!");
            }
        }

        public async Task<ResponseModel> SendMessage(SendMessageModel model)
        {
            try
            {
                if(model.IsToGroup && model.GroupId == null)
                {
                    return new ResponseModel(404, "Grupo não encontrado!");
                }

                if (model.IsToGroup)
                {
                    var group = await _db.Groups.FirstOrDefaultAsync(x => x.Id == model.GroupId);

                    if (group == null)
                    {
                        return ResponseModel.BuildNotFoundResponse("Grupo não encontrado");
                    }

                    ChatGroupMessage groupMessage = new() 
                    {
                        GroupId = group.Id,
                        Content = model.Message,
                        SenderUserId = model.SenderUserId,
                        Type = model.Type,
                    };

                    await _db.AddAsync(groupMessage);
                    await _db.SaveChangesAsync();

                    string wsResponse = JsonConvert.SerializeObject(new SendMessageSocketModel() { GroupMessage = groupMessage, GroupMessageId = groupMessage.Id }, new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                    
                    await _ws.SendMessageToGroup(group.Id.ToString(), wsResponse);
                    await _ws.SendMessageToGroup(model.RecipientUserId, wsResponse);

                    return ResponseModel.BuildOkResponse("Operação realizada com sucesso!");
                }
                else
                {
                    Chat? chat = await _db.Chats.FirstOrDefaultAsync(x => x.Id == model.ChatId);

                    if (chat == null)
                    {
                        return new ResponseModel(404, "Chat não encontrado!");
                    }

                    ChatMessage message = new()
                    {
                        ChatId = chat.Id,
                        Content = model.Message,
                        SenderUserId = model.SenderUserId,
                        Type = model.Type,
                    };

                    await _db.AddAsync(message);
                    await _db.SaveChangesAsync();


                    string wsResponse = System.Text.Json.JsonSerializer.Serialize(new SendMessageSocketModel() { Message = message, MessageId = message.Id }, new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.IgnoreCycles,
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                        WriteIndented = true
                    });

                    await _ws.SendMessageToGroup(chat.Id.ToString(), wsResponse);
                    await _ws.SendMessageToGroup(model.RecipientUserId, wsResponse);

                    return ResponseModel.BuildOkResponse("Operação realizada com sucesso!", new { chatId = chat.Id });
                }
            }
            catch
            {
                return new ResponseModel(500, "Ocorreu um erro!");
            }
        }
    }
}
