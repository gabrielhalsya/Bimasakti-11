using R_APICommonDTO;
using System.Collections.Generic;

namespace GLM00200COMMON
{
    public class GLM00200ListResult<T> : R_APIResultBaseDTO
    {
        public List<T> Data { get; set; }
    }

    public class GLM00200RecordResult<T> : R_APIResultBaseDTO
    {
        public T Data { get; set; }
    }
}
