using FundooMicroservices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooMicroservices.Shared.Interfaces
{
    public interface INoteServices
    {
        Task<NoteModel> AddNoteAsync(NoteModel newNoteDetails, string email);

        Task<List<NoteModel>> GetNotes(string email);

        Task<List<NoteModel>> UpdateNoteAsync(NoteModel updatedNoteDetails, string email);

        Task DeleteNoteAsync(string noteId, string email);

    }
}
