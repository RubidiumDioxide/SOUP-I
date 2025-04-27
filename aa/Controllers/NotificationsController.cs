using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using aa.Models;
using aa.Views; 

namespace aa.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly SoupDbContext context;

        public NotificationsController(SoupDbContext _context)
        {
            context = _context;
        }

        // GET: api/Notifications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetNotifications()
        {
            return await context.Notifications
                .Select(n => new NotificationDto(n))
                .ToListAsync();
        }

        // GET: api/Notifications/ByReceiver/5
        [HttpGet("ByReceiver/{receiverId}")]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetNotificationsByReceiver(int receiverId)
        {
            return await context.Notifications
                .Where(n => n.ReceiverId == receiverId)
                .Select(n => new NotificationDto(n))
                .ToListAsync();
        }


        // GET: api/Notifications/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NotificationDto>> GetNotification(int id)
        {
            var notification = await context.Notifications.FindAsync(id);

            if (notification == null)
            {
                return NotFound();
            }

            return new NotificationDto(notification);
        }

        // will likely go unused 
        // PUT: api/Notifications/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNotification(int id, NotificationDto notificationdto)
        {
            if (id != notificationdto.Id)
            {
                return BadRequest();
            }

            var notification = await context.Notifications.FindAsync(id); 

            if(notification == null)
            {
                return NotFound();
            }

            notification.ProjectId = notificationdto.ProjectId; 
            notification.SenderId = notificationdto.SenderId;
            notification.ReceiverId = notificationdto.ReceiverId; 
            notification.Role = notificationdto.Role; 
            notification.Type = notificationdto.Type; 

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (! NotificationExists(id))
            {
                return NotFound(); 
            }

            return NoContent();
        }

        // POST: api/Notifications
        [HttpPost]
        public async Task<IActionResult> PostNotification(NotificationDto notificationdto)
        {
            var notification = new Notification
            {
                ProjectId = notificationdto.ProjectId, 
                SenderId = notificationdto.SenderId, 
                ReceiverId = notificationdto.ReceiverId, 
                Role = notificationdto.Role, 
                Type = notificationdto.Type, 
            };

            context.Notifications.Add(notification);

            await context.SaveChangesAsync(); 

            return NoContent();
        }

        // POST: api/Notifications/{userName} 
        [HttpPost("{receiverName}")]
        public async Task<IActionResult> PostNotificationByName(NotificationDto notificationdto, string receiverName)
        {
            var receiver = context.Users.FirstOrDefault(u => u.Name == receiverName); 

            if(receiver == null)
            {
                return NotFound(); 
            }

            var notification = new Notification
            {
                ProjectId = notificationdto.ProjectId,
                SenderId = notificationdto.SenderId, 
                ReceiverId = receiver.Id, 
                Role = notificationdto.Role,
                Type = notificationdto.Type,
            };

            context.Notifications.Add(notification);

            await context.SaveChangesAsync();

            return NoContent();
        }


        // DELETE: api/Notifications/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            var notification = await context.Notifications.FindAsync(id);
            
            if (notification == null)
            {
                return NotFound();
            }

            context.Notifications.Remove(notification);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool NotificationExists(int id)
        {
            return context.Notifications.Any(e => e.Id == id);
        }
    }
}
