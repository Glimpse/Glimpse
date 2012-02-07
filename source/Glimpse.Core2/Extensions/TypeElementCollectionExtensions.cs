using System;
using System.Collections.Generic;
using Glimpse.Core2.Configuration;

namespace Glimpse.Core2.Extensions
{
    public static class TypeElementCollectionExtensions
    {
         public static IEnumerable<Type> ToEnumerable(this TypeElementCollection collection)
         {
             foreach (TypeElement typeElement in collection)
             {
                 yield return typeElement.Type;
             }
         }
    }
}