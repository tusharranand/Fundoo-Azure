using FundooMicroservices.Models;
using FundooMicroservices.Shared.Interfaces;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooMicroservices.Shared.Services
{
    public class NoteServices : INoteServices
    {
        private readonly ISettingServices _settingServices;

        // Cosmos DocDB API database
        private string _docDbEndpointUri;
        private string _docDbPrimaryKey;
        private string _docDbDatabaseName;

        // Doc DB Collections
        private string _docDbDigitalMainCollectionName;

        private static CosmosClient _docDbSingletonClient;
        private readonly Container _cosmosContainer;

        public NoteServices(ISettingServices settingServices)
        {
            _settingServices = settingServices;

            _docDbEndpointUri = _settingServices.GetDocDbEndpointUri();
            _docDbPrimaryKey = _settingServices.GetDocDbApiKey();
            _docDbDatabaseName = _settingServices.GetDocDbDatabaseName();
            _docDbDigitalMainCollectionName = _settingServices.GetDocDbMainCollectionName();
            _docDbSingletonClient = new CosmosClient(_settingServices.GetDocDbEndpointUri(), settingServices.GetDocDbApiKey());
            _cosmosContainer = _docDbSingletonClient.GetContainer(_docDbDatabaseName, _docDbDigitalMainCollectionName);

        }

        public async Task<NoteModel> AddNoteAsync(NoteModel newNoteDetails, string email)
        {
            try
            {
                if (newNoteDetails == null)
                    throw new ArgumentNullException(nameof(newNoteDetails));

                newNoteDetails.Collaborators.Add(email);
                newNoteDetails.NoteId = Guid.NewGuid().ToString();
                newNoteDetails.CreationDate = DateTime.Now;

                using (var response = _cosmosContainer
                    .CreateItemAsync(newNoteDetails,
                    new PartitionKey(newNoteDetails.NoteId)))
                {
                    return response.Result.Resource;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<NoteModel>> GetNotes(string email)
        {
            try
            {
                var response = _cosmosContainer.GetItemLinqQueryable<NoteModel>(true)
                    .Where(x => x.Collaborators.Contains(email));
                return response.ToList<NoteModel>();
            }
            catch (Exception)
            {
                throw;
            }        
        }

        public async Task<List<NoteModel>> UpdateNoteAsync(NoteModel updatedNoteDetails, string email)
        {
            try
            {
                var origNote = _cosmosContainer.GetItemLinqQueryable<NoteModel>(true)
                    .Where(x => x.Collaborators.Contains(email) && x.NoteId == updatedNoteDetails.NoteId)
                    .AsEnumerable().FirstOrDefault();
                if (origNote == null)
                    throw new ArgumentNullException(nameof(origNote));

                await _cosmosContainer.ReplaceItemAsync<NoteModel>(updatedNoteDetails, origNote.NoteId, 
                    new PartitionKey(updatedNoteDetails.NoteId));
                List<NoteModel> result = new List<NoteModel>();
                result.Add(origNote);
                result.Add(updatedNoteDetails);

                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteNoteAsync(string noteId, string email)
        {
            try
            {
                var response = _cosmosContainer.GetItemLinqQueryable<NoteModel>(true)
                    .Where(x => x.Collaborators.Contains(email) && x.NoteId == noteId).AsEnumerable().FirstOrDefault();
                if (response == null)
                    throw new ArgumentNullException(nameof(response));

                await _cosmosContainer.DeleteItemAsync<NoteModel>(response.NoteId, new PartitionKey(response.NoteId));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
