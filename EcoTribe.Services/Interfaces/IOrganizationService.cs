﻿using EcoTribe.BusinessObjects.InputModels;
using EcoTribe.BusinessObjects.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoTribe.Services.Interfaces
{
    public interface IOrganizationService : IService<OrganizationViewModel, OrganizationInputModel>
    {
        void Create(OrganizationInputModel inputModel);
        void Update(int id, OrganizationInputModel inputModel);
        void Delete(int id);
    }
}
