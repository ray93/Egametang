﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Base
{
	public abstract class ACategory<T>: ICategory where T : AConfig
	{
		protected Dictionary<long, T> dict;

		public virtual void BeginInit()
		{
			this.dict = new Dictionary<long, T>();

			string path = $@"Config/{typeof (T).Name}";
			string configStr;
			try
			{
				configStr = ((GameObject)Resources.Load("Config")).Get<TextAsset>(typeof(T).Name).text;
			}
			catch (Exception)
			{
				throw new GameException($"load config file fail, path: {path}");
			}

			foreach (string str in configStr.Split(new[] { "\r\n" }, StringSplitOptions.None))
			{
				try
				{
					string str2 = str.Trim();
					if (str2 == "")
					{
						continue;
					}
					T t = MongoHelper.FromJson<T>(str2);
					this.dict.Add(t.id, t);
				}
				catch (Exception e)
				{
					throw new GameException($"parser json fail: {str}", e);
				}
			}
		}

		public Type ConfigType
		{
			get
			{
				return typeof (T);
			}
		}

		public virtual void EndInit()
		{
		}

		public T this[long type]
		{
			get
			{
				T t;
				if (!this.dict.TryGetValue(type, out t))
				{
					throw new ConfigException($"{typeof (T)} 没有找到配置, key: {type}");
				}
				return t;
			}
		}

		public T TryGet(int type)
		{
			T t;
			if (!this.dict.TryGetValue(type, out t))
			{
				return null;
			}
			return t;
		}

		public T[] GetAll()
		{
			return this.dict.Values.ToArray();
		}

		public T GetOne()
		{
			return this.dict.Values.First();
		}
	}
}