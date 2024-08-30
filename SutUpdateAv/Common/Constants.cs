using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using SutUpdateAv.Models;

namespace SutUpdateAv.Common
{
    public static class Constants
    {
        //List<Item> choiceViewDevList = new List<Item> 
        //{ 
        //    new Item { Id = 0, Value = "Все" }, 
        //    new Item { Id = 1, Value = "Не назначенные" }, 
        //    new Item { Id = 2, Value = "Назначенные" }
        //};

        //public static List<Item> ChoiceViewDevList
        //{
        //    get 
        //    {
        //        return new List<Item>
        //        {
        //            new Item { Id = 0, Value = "Все" },
        //            new Item { Id = 1, Value = "Не назначенные" },
        //            new Item { Id = 2, Value = "Назначенные" }
        //        };
        //    }
        //}


        // убрать отсюда?
        public static List<Item> ChoiceViewDevList = new List<Item>
        {
            new Item { Id = 0, Value = "Все" },
            new Item { Id = 1, Value = "Не назначенные" },
            new Item { Id = 2, Value = "Назначенные" }
        };


        public static string GetDescriptionValue(Enum value)
        {
          
            Type type = value.GetType();

            
            FieldInfo fieldInfo = type.GetField(value.ToString());

         
            DescriptionAttribute[] attribs = fieldInfo.GetCustomAttributes(
                typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            return attribs.Length > 0 ? attribs[0].Description : null;
        }

        public static string GetDescriptionValue(Type type, string value)
        {
        
            PropertyInfo fieldInfo = type.GetProperty(value);

         
            DescriptionAttribute[] attribs = fieldInfo.GetCustomAttributes(
                typeof(DescriptionAttribute), false) as DescriptionAttribute[];

        
            return attribs.Length > 0 ? attribs[0].Description : null;
        }
    }



}
