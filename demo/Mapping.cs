using AutoMapper;
using demo.Models;
using demo.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace demo
{
    public class Mapping
    {
        public static IMapperConfigurationExpression MappingBuilder(IMapperConfigurationExpression builder)
        {
            builder.CreateMap<User, UserView>();
            return builder;
        }
    }
}
