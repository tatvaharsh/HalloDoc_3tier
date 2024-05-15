using hallocdoc_mvc_Service.Interface;
using hallodoc_mvc_Repository.DataContext;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Graph.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hallodoc_mvc_Repository.ViewModel;
using hallodoc_mvc_Repository.DataModels;

namespace hallocdoc_mvc_Service.Implementation
{

    public class ChatHub : Hub
    {

        private readonly ApplicationDbContext _context;
        private readonly IGenericRepository<hallodoc_mvc_Repository.DataModels.Chat> _chatRepo;

        public ChatHub(ApplicationDbContext context, IGenericRepository<hallodoc_mvc_Repository.DataModels.Chat> chatRepo)
        {
            _context = context;
            _chatRepo = chatRepo;
        }

        public async Task SendMessage(string message, string RequestID, string ProviderID, string AdminID, string RoleID, string GroupFlagID)
        {
            hallodoc_mvc_Repository.DataModels.Chat chat = new hallodoc_mvc_Repository.DataModels.Chat();
            chat.Message = message;
            chat.SentBy = Convert.ToInt32(RoleID);
            chat.AdminId = Convert.ToInt32(AdminID);
            chat.RequestId = Convert.ToInt32(RequestID);
            chat.PhyscainId = Convert.ToInt32(ProviderID);
            chat.SentDate = DateTime.Now;
            chat.ChatType = Convert.ToInt32(GroupFlagID);
            _chatRepo.Add(chat);

            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}