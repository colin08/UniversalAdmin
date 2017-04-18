using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;

namespace Universal.Tests
{
    /// <summary>
    /// 对象映射测试
    /// </summary>
    [TestClass]
    public class AutoMapperTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            #region 基础转换，并对额外的字段做处理

            Name name1 = new Name() { FirstName = null, LastName = "XiYou" };
            Mapper.Initialize(cfg =>
                cfg.CreateMap<Name, NameDto>()
                .ForMember(x => x.FirstName, s => s.NullSubstitute("Default"))//默认值
                .ForMember(x => x.AllName, s => s.MapFrom(src => src.FirstName + " " + src.LastName))
                );
            var name2 = Mapper.Map<Name, NameDto>(name1);

            #endregion

            //#region 包含子对象，且指定类名

            //var config = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<OuterSource, OuterDest>();
            //    cfg.CreateMap<InnerSource, InnerDest>();
            //});
            //config.AssertConfigurationIsValid();
            //var source = new OuterSource()
            //{
            //    Value = 5,
            //    Inner = new InnerSource() { OtherValue = 18 }
            //};
            //var mapper = config.CreateMapper();
            //var dest = mapper.Map<OuterSource, OuterDest>(source);

            //#endregion

            //Mapper.Initialize(cfg =>
            //{
            //    cfg.CreateMap<string, int>().ConvertUsing(Convert.ToInt32);
            //    cfg.CreateMap<string, DateTime>().ConvertUsing(new DateTimeTypeConverter());
            //    cfg.CreateMap<string, Type>().ConvertUsing<TypeTypeConverter>();
            //    cfg.CreateMap<Source, Destination>();
            //});

            //Mapper.AssertConfigurationIsValid();

            //var source = new Source
            //{
            //    Value1 = "5",
            //    Value2 = "01/01/2000",
            //    Value3 = "Universal.Tests.Destination"
            //};

            //Destination result = Mapper.Map<Source, Destination>(source);

            Assert.AreEqual(1, 1);
        }
    }
    #region 基础转换，并对额外的字段做处理

    public class Name
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }


    public class NameDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AllName { get; set; }
    }

    #endregion

    #region 包含子对象，且指定类名

    public class OuterSource
    {
        public int Value { get; set; }
        public InnerSource Inner { get; set; }
    }

    public class InnerSource
    {
        public int OtherValue { get; set; }
    }

    public class OuterDest
    {
        public int Value { get; set; }
        public InnerDest Inner { get; set; }
    }

    public class InnerDest
    {
        public int OtherValue { get; set; }
    }

    #endregion

    #region 指定转换类型

    public class Source
    {
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }
    }

    public class Destination
    {
        public int Value1 { get; set; }
        public DateTime Value2 { get; set; }
        public Type Value3 { get; set; }
    }

    public class DateTimeTypeConverter : ITypeConverter<string, DateTime>
    {
        public DateTime Convert(string source, DateTime destination, ResolutionContext context)
        {
            return System.Convert.ToDateTime(source);
        }
    }

    public class TypeTypeConverter : ITypeConverter<string, Type>
    {
        public Type Convert(string source, Type destination, ResolutionContext context)
        {
            return context.GetType();
        }
    }

    #endregion

}
