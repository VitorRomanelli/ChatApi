using ChatApi.Entities;
using ChatApi.Models;
using ChatApi.Services.Interfaces;
using ChatApi.WebSocket.Handlers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ChatApi.Services
{
    public class ChatService : IChatService
    {
        private readonly RoomHandler _ws;

        public ChatService(RoomHandler ws)
        {
            _ws = ws;
        } 

        public async Task<ResponseModel> EnterChat(string roomId, User user)
        {
            try
            {
                await _ws.SendMessageToGroup(roomId, JsonConvert.SerializeObject(new RoomSocketModel() { Type = "EnterChat", User = user }, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }));

                return new ResponseModel(200, "Operação realizada com sucesso!");
            }
            catch
            {
                return new ResponseModel(500, "Ocorreu um erro ao entrar na sala!");
            }
        }

        public async Task<ResponseModel> SendMessage(string roomId, User user, string message)
        {
            try
            {
                await _ws.SendMessageToGroup(roomId, JsonConvert.SerializeObject(new RoomSocketModel() { Type = "EnterChat", User = user, Message = message}, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }));

                return new ResponseModel(200, "Operação realizada com sucesso!");
            }
            catch
            {
                return new ResponseModel(500, "Ocorreu um erro ao entrar na sala!");
            }
        }
    }
}
