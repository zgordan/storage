﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MtdKey.Storage
{
    public partial class RequestProvider : IDisposable
    {
        public async Task<RequestResult<BunchFields>> GetSchemaAsync(string bunchName)
        {
            var bunchQueryAsync = async () => await BunchQueryAsync(filter =>
            {
                filter.SearchText = bunchName;
                filter.PageSize = int.MaxValue;
            });

            return await GetScheamaAsync(bunchQueryAsync);
        }


        public async Task<RequestResult<BunchFields>> GetScheamaAsync(long bunchId)
        {
            var bunchQueryAsync = async () => await BunchQueryAsync(filter =>
            {
                filter.BunchIds.Add(bunchId);
                filter.PageSize = int.MaxValue;
            });

            return await GetScheamaAsync(bunchQueryAsync);
        }

        public async Task<RequestResult<BunchFields>> GetSchemaAsync()
        {
            return await GetScheamaAsync(async () => await BunchQueryAsync(filter => filter.PageSize = int.MaxValue));
        }

        private async Task<RequestResult<BunchFields>> GetScheamaAsync(Func<Task<RequestResult<BunchPattern>>> bunchQueryAsync)
        {
            var result = new RequestResult<BunchFields>(true);
            var data = new List<BunchFields>();

            var bunchesReturned = await bunchQueryAsync();

            if (!bunchesReturned.Success)
                return new RequestResult<BunchFields>(false, bunchesReturned.Exception);

            var bunches = bunchesReturned.DataSet;
            foreach (var bunch in bunches)
            {
                var fieldsReturned = await FieldQueryAsync(filter => { 
                    filter.BunchIds.Add(bunch.BunchId);
                    filter.PageSize = int.MaxValue;
                });

                if (!fieldsReturned.Success)
                    return new RequestResult<BunchFields>(false, fieldsReturned.Exception);

                data.Add(new BunchFields()
                {
                    BunchPattern = bunch,
                    FieldPatterns = fieldsReturned.DataSet
                });
            }

            result.FillDataSet(data);

            return result;
        }
    }
}
