using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CopyRecipeBookMVC.Application.Mapping;
using CopyRecipeBookMVC.Domain.Model;

namespace CopyRecipeBookMVC.Application.ViewModels.Time
{
    public class TimeForListVm :IMapFrom<Domain.Model.Time>
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public string Unit { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Model.Time, TimeForListVm>();
        }
    }
}
