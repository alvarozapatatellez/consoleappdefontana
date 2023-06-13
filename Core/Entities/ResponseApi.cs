using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Core.Entities
{
	public class ResponseApi
	{
		public bool IsSuccess { get; set; }
		public string? Message { get; set; }
		public object? Result { get; set; }
	}


	public class ResultApi<T>
	{
		public bool IsSuccess { get; set; } = false;
		public int RecordsUpdate { get; set; } = 0;
		public  T? DataItem { get; set; }
		public string ScalarValue { get; set; } = string.Empty;
		public IEnumerable<T>? DataList { get; set; }
		public List<T>? DATA { get; set; }
		public int ErrorNum { get; set; } = 0;
		public string ErrorDesc { get; set; } = string.Empty;
		public string ErrorMsj { get; set; } = string.Empty;
		public string InnerError { get; set; } = string.Empty;
	}

	// Clase para el objeto raíz del JSON
	public class RootObject<T>
	{
		public List<T>? DATA { get; set; }

	}
}
