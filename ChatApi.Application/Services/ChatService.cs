using ChatApi.Application.Models;
using ChatApi.Application.Services.Interfaces;
using ChatApi.Domain.Entities;
using ChatApi.Persistence.Data;
using ChatApi.WebSocket.Handlers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
                    ChatGroupMessage groupMessage = new() 
                    {
                        GroupId = (Guid)model.GroupId,
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
                    await _ws.SendMessageToGroup(groupMessage.Id.ToString(), wsResponse);
                }
                else
                {
                    Chat? chat = await _db.Chats.FirstOrDefaultAsync(x => x.Id == model.ChatId);

                    if (chat == null)
                    {
                        chat = new Chat();
                        await _db.AddAsync(chat);
                    }

                    ChatMessage message = new()
                    {
                        ChatId = chat.Id,
                        Content = model.Message,
                        RecipientUserId = model.RecipientUserId,
                        SenderUserId = model.SenderUserId,
                        Type = model.Type,
                    };

                    await _db.AddAsync(message);
                    await _db.SaveChangesAsync();

                    string wsResponse = JsonConvert.SerializeObject(new SendMessageSocketModel() { Message = message, MessageId = message.Id }, new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                    await _ws.SendMessageToGroup(model.SenderUserId, wsResponse);
                }

                return new ResponseModel(200, "Operação realizada com sucesso!");
            }
            catch
            {
                return new ResponseModel(500, "Ocorreu um erro ao entrar na sala!");
            }
        }
    }
}
