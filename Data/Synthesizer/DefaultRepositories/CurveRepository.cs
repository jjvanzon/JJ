﻿using System.Collections.Generic;
using JJ.Framework.Data;
using JJ.Data.Synthesizer.Entities;
using JJ.Data.Synthesizer.RepositoryInterfaces;

namespace JJ.Data.Synthesizer.DefaultRepositories
{
	public class CurveRepository : RepositoryBase<Curve, int>, ICurveRepository
	{
		// ReSharper disable once MemberCanBeProtected.Global
		public CurveRepository(IContext context)
			: base(context)
		{ }

		public virtual IList<Curve> GetAll() => throw new RepositoryMethodNotImplementedException();
	}
}
