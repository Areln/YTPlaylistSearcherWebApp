using Microsoft.EntityFrameworkCore;
using YTPlaylistSearcherWebApp.Data;
using YTPlaylistSearcherWebApp.DTOs;
using YTPlaylistSearcherWebApp.Models;
using YTPlaylistSearcherWebApp.Repositories;

namespace YTPlaylistSearcherWebApp.Services
{
    public class HoneyMediaService : IHoneyMediaService
    {
        private readonly IHoneyMediaRepository _honeyMediaRepository;

        public HoneyMediaService(IHoneyMediaRepository honeyMediaRepository)
        {
            _honeyMediaRepository = honeyMediaRepository;
        }

        public async Task<IEnumerable<HoneyMediaDTO>> GetAllHoneyMedia(YTPSContext context)
        {
            var entities = await _honeyMediaRepository.GetAllHoneyMedia(context);
            return entities.Select(MapToDTO);
        }

        public async Task<HoneyMediaDTO?> GetHoneyMediaById(YTPSContext context, int id)
        {
            var entity = await _honeyMediaRepository.GetHoneyMediaById(context, id);
            return entity != null ? MapToDTO(entity) : null;
        }

        public async Task<HoneyMediaDTO> CreateHoneyMedia(YTPSContext context, HoneyMediaDTO dto)
        {
            var entity = MapToModel(dto);
            await _honeyMediaRepository.AddHoneyMedia(context, entity);
            await context.SaveChangesAsync();
            return MapToDTO(entity);
        }

        public async Task<HoneyMediaDTO?> UpdateHoneyMedia(YTPSContext context, int id, HoneyMediaDTO dto)
        {
            var entity = await _honeyMediaRepository.GetHoneyMediaById(context, id);
            if (entity == null)
                return null;

            entity.MediaTitle = dto.MediaTitle;
            entity.Pitch = dto.Pitch;
            entity.MediaTypeId = dto.MediaTypeId;
            entity.InterestTypeId = dto.InterestTypeId;
            entity.RequestingUserId = dto.RequestingUserId;
            entity.ResponseId = dto.ResponseId;
            entity.StatusId = dto.StatusId;
            entity.NoelleRating = dto.NoelleRating;
            entity.AaronRating = dto.AaronRating;
            entity.NoelleComment = dto.NoelleComment;
            entity.AaronComment = dto.AaronComment;
            entity.Tracker = dto.Tracker;
            entity.DateRequested = dto.DateRequested;
            entity.DateFinished = dto.DateFinished;

            await _honeyMediaRepository.UpdateHoneyMedia(context, entity);
            await context.SaveChangesAsync();
            return MapToDTO(entity);
        }

        public async Task<bool> DeleteHoneyMedia(YTPSContext context, int id)
        {
            var entity = await _honeyMediaRepository.GetHoneyMediaById(context, id);
            if (entity == null)
                return false;

            await _honeyMediaRepository.DeleteHoneyMedia(context, entity);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<MediaTypeDTO>> GetMediaTypes(YTPSContext context)
        {
            var entities = await _honeyMediaRepository.GetMediaTypes(context);
            return entities.Select(x => new MediaTypeDTO { Id = x.Id, MediaTypeName = x.MediaTypeName, Color = x.Color });
        }

        public async Task<IEnumerable<MediaInterestDTO>> GetMediaInterests(YTPSContext context)
        {
            var entities = await _honeyMediaRepository.GetMediaInterests(context);
            return entities.Select(x => new MediaInterestDTO { Id = x.Id, InterestTypeName = x.InterestTypeName, Color = x.Color });
        }

        public async Task<IEnumerable<MediaRequesterDTO>> GetMediaRequesters(YTPSContext context)
        {
            var entities = await _honeyMediaRepository.GetMediaRequesters(context);
            return entities.Select(x => new MediaRequesterDTO { Id = x.Id, Name = x.Name, Color = x.Color });
        }

        public async Task<IEnumerable<MediaResponseDTO>> GetMediaResponses(YTPSContext context)
        {
            var entities = await _honeyMediaRepository.GetMediaResponses(context);
            return entities.Select(x => new MediaResponseDTO { Id = x.Id, Response = x.Response, Color = x.Color });
        }

        public async Task<IEnumerable<MediaStatusDTO>> GetMediaStatuses(YTPSContext context)
        {
            var entities = await _honeyMediaRepository.GetMediaStatuses(context);
            return entities.Select(x => new MediaStatusDTO { Id = x.Id, Status = x.Status, Color = x.Color });
        }

        private static HoneyMediaDTO MapToDTO(Honeymedia entity)
        {
            return new HoneyMediaDTO
            {
                Id = entity.Id,
                MediaTitle = entity.MediaTitle,
                Pitch = entity.Pitch,
                MediaTypeId = entity.MediaTypeId,
                MediaTypeName = entity.MediaType?.MediaTypeName,
                MediaTypeColor = entity.MediaType?.Color,
                InterestTypeId = entity.InterestTypeId,
                InterestTypeName = entity.InterestType?.InterestTypeName,
                InterestTypeColor = entity.InterestType?.Color,
                RequestingUserId = entity.RequestingUserId,
                RequestingUserName = entity.RequestingUser?.Name,
                RequestingUserColor = entity.RequestingUser?.Color,
                ResponseId = entity.ResponseId,
                Response = entity.Response?.Response,
                ResponseColor = entity.Response?.Color,
                StatusId = entity.StatusId,
                Status = entity.Status?.Status,
                StatusColor = entity.Status?.Color,
                NoelleRating = entity.NoelleRating,
                AaronRating = entity.AaronRating,
                NoelleComment = entity.NoelleComment,
                AaronComment = entity.AaronComment,
                Tracker = entity.Tracker,
                DateRequested = entity.DateRequested,
                DateFinished = entity.DateFinished,
                LastUpdated = entity.LastUpdated
            };
        }

        private static Honeymedia MapToModel(HoneyMediaDTO dto)
        {
            return new Honeymedia
            {
                Id = dto.Id,
                MediaTitle = dto.MediaTitle,
                Pitch = dto.Pitch,
                MediaTypeId = dto.MediaTypeId,
                InterestTypeId = dto.InterestTypeId,
                RequestingUserId = dto.RequestingUserId,
                ResponseId = dto.ResponseId,
                StatusId = dto.StatusId,
                NoelleRating = dto.NoelleRating,
                AaronRating = dto.AaronRating,
                NoelleComment = dto.NoelleComment,
                AaronComment = dto.AaronComment,
                Tracker = dto.Tracker,
                DateRequested = dto.DateRequested,
                DateFinished = dto.DateFinished
            };
        }
    }

    public interface IHoneyMediaService
    {
        Task<IEnumerable<HoneyMediaDTO>> GetAllHoneyMedia(YTPSContext context);
        Task<HoneyMediaDTO?> GetHoneyMediaById(YTPSContext context, int id);
        Task<HoneyMediaDTO> CreateHoneyMedia(YTPSContext context, HoneyMediaDTO dto);
        Task<HoneyMediaDTO?> UpdateHoneyMedia(YTPSContext context, int id, HoneyMediaDTO dto);
        Task<bool> DeleteHoneyMedia(YTPSContext context, int id);
        Task<IEnumerable<MediaTypeDTO>> GetMediaTypes(YTPSContext context);
        Task<IEnumerable<MediaInterestDTO>> GetMediaInterests(YTPSContext context);
        Task<IEnumerable<MediaRequesterDTO>> GetMediaRequesters(YTPSContext context);
        Task<IEnumerable<MediaResponseDTO>> GetMediaResponses(YTPSContext context);
        Task<IEnumerable<MediaStatusDTO>> GetMediaStatuses(YTPSContext context);
    }
}

