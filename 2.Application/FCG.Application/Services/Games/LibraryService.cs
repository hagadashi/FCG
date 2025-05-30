using AutoMapper;
using FCG.Application.DTOs.Games;
using FCG.Application.Interfaces.Services.Games;
using FCG.Domain.Interfaces.Repositories.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Application.Services.Games
{
    public class LibraryService : ILibraryService
    {
        private readonly ILibraryRepository _libraryRepository;
        private readonly IMapper _mapper;

        public LibraryService(ILibraryRepository libraryRepository,
            IMapper mapper)
        {
            _libraryRepository = libraryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LibraryDto>> GetLibrariesByUserAsync(Guid userId)
        {
            var libs = await _libraryRepository.GetByUserIdAsync(userId);
            var libsDto = _mapper.Map<IEnumerable<LibraryDto>>(libs);
            return libsDto;
        }
    }
}
