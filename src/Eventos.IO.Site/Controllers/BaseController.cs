﻿using Eventos.IO.Domain.Core.Notifications;
using Eventos.IO.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Eventos.IO.Site.Controllers
{
    public class BaseController : Controller
    {
        private readonly IDomainNotificationHandler<DomainNotification> _notifications;
        private readonly IUser _user;

        public Guid OrganizadorId { get; set; }

        public BaseController(IDomainNotificationHandler<DomainNotification> notifications,
            IUser user)
        {
            _notifications = notifications;

            if (user.IsAuthenticated())
            {
                OrganizadorId = user.GetUserId();
            }
        }

        protected bool OperacaoValida()
        {
            return (!_notifications.HasNotifications());
        }

    }
}
