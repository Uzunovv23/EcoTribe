﻿using EcoTribe.BusinessObjects.InputModels;
using EcoTribe.BusinessObjects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.Services.Interfaces
{
    public interface IEventService
    {
        IEnumerable<EventViewModel> GetAll();
        void Create(EventInputModel inputModel);
    }
}
