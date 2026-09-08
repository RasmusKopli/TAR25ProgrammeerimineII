using System;
using System.Collections.Generic;
using System.Text;
using ShopTARpe25.Core.Domain;
using ShopTARpe25.Core.Dto;

namespace ShopTARpe25.Core.ServiceInterface
{
    public interface ISpaceshipServices
    {
        Task<Spaceship> Create(SpaceshipDto dto);
    }
}
