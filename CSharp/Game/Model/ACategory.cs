﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.Helper;

namespace Model
{
	public abstract class ACategory<T>: ICategory where T : AConfig
	{
		private Dictionary<int, T> dict;

		public virtual void BeginInit()
		{
			this.dict = new Dictionary<int, T>();

			string path = Path.Combine(@"../../Config/", typeof (T).Name);

			if (!Directory.Exists(path))
			{
				throw new Exception($"not found config path: {path}");
			}

			foreach (string file in Directory.GetFiles(path))
			{
				T t = MongoHelper.FromJson<T>(File.ReadAllText(file));
				this.dict.Add(t.Id, t);
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

		public T this[int type]
		{
			get
			{
				return this.dict[type];
			}
		}

		public T[] GetAll()
		{
			return this.dict.Values.ToArray();
		}
	}
}