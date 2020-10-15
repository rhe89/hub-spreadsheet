﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Spreadsheet.Dto.Spreadsheet;
using Spreadsheet.Integration;
using Spreadsheet.Providers;
using Spreadsheet.Shared.Exceptions;

namespace Spreadsheet.SpreadsheetTabReaders
{
    public class TabReader<TTabDto> : ITabReader<TTabDto> where TTabDto : TabDto, new()
    {
        private readonly IGoogleSpreadsheetConnector _googleSpreadsheetConnector;
        private readonly ISpreadsheetProvider _spreadsheetProvider;
        private readonly string _tabName;

        public TabReader(ISpreadsheetProvider spreadsheetProvider, 
            IGoogleSpreadsheetConnector googleSpreadsheetConnector,
            string tabName)
        {
            _googleSpreadsheetConnector = googleSpreadsheetConnector;
            _tabName = tabName;
            _spreadsheetProvider = spreadsheetProvider;
        }
        
        public async Task<TTabDto> GetTab()    
        {
            var apiDataTabDto = await SetTabDtoFromCurrentBudgetSpreadsheet();
            
            await PopulateRowsInTabDto(apiDataTabDto);

            return apiDataTabDto;
        }

        private async Task<TTabDto> SetTabDtoFromCurrentBudgetSpreadsheet()
        {
            var spreadsheetMetadataDto = await GetCurrentBudgetSpreadsheetMetadataDto();

            return SetTabDtoFromSpreadsheetMetadata(spreadsheetMetadataDto, _tabName);
        }

        private async Task<SpreadsheetMetadataDto> GetCurrentBudgetSpreadsheetMetadataDto()
        {
            var spreadsheetMetadataDto = await _spreadsheetProvider.CurrentBudgetSpreadsheet();
            
            if (spreadsheetMetadataDto == null)
            {
                throw new SpreadsheetNotFoundException("No metadata exists for budget spreadsheet for current date");
            }

            return spreadsheetMetadataDto;
        }

        private static TTabDto SetTabDtoFromSpreadsheetMetadata(SpreadsheetMetadataDto spreadSheet, string tabName)
        {
            var spreadsheetTabMetadata =
                spreadSheet.SpreadsheetTabMetadataDtos.FirstOrDefault(x =>
                    x.Name == tabName);

            if (spreadsheetTabMetadata == null)
            {
                throw new ArgumentException(
                    $"No metadata exists for {tabName} in SpreadsheetMetadata with id {spreadSheet.Id}",
                    nameof(tabName));
            }

            return new TTabDto
            {
                Name = spreadsheetTabMetadata.Name,
                SpreadsheetId = spreadSheet.SpreadsheetId,
                FirstColumn = spreadsheetTabMetadata.FirstColumn,
                LastColumn = spreadsheetTabMetadata.LastColumn,
                SpreadsheetRowMetadataDtos = spreadsheetTabMetadata.SpreadsheetRowMetadataDtos
            };
        }

        private async Task PopulateRowsInTabDto(TabDto tabDto)
        {
            await _googleSpreadsheetConnector.LoadSpreadsheetTab(tabDto);
        }
    }
}