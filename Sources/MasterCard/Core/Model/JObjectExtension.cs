﻿using System;
using System.Linq;
using Newtonsoft.Json.Linq;


using System.Collections.Generic;

namespace MasterCard.Core.Model
{
	public static class JObjectExtensions
	{
		public static IDictionary<string, object> ToDictionary(this JObject @object)
		{
			var result = @object.ToObject<Dictionary<string, object>>();

			var JObjectKeys = (from r in result
				let key = r.Key
				let value = r.Value
				where value.GetType() == typeof(JObject)
				select key).ToList();

			var JArrayKeys = (from r in result
				let key = r.Key
				let value = r.Value
				where value.GetType() == typeof(JArray)
				select key).ToList();

			JArrayKeys.ForEach(key => result[key] = ((JArray)result[key]).Values().Select(x => ((JValue)x).Value).ToArray());
			JObjectKeys.ForEach(key => result[key] = ToDictionary(result[key] as JObject));

			return result;
		}
	}
}

