using OnlineStore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Services
{
    public interface IMailService
    {
        Task Send(Message message);
    }
}
