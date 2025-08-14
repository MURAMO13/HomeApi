using AutoMapper;
using HomeApi.Contracts.Models.Rooms;
using HomeApi.Data.Models;
using HomeApi.Data.Repos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HomeApi.Controllers
{
    /// <summary>
    /// Контроллер комнат
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RoomsController : ControllerBase
    {
        private IRoomRepository _repository;
        private IMapper _mapper;

        public RoomsController(IRoomRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        //TODO: Задание - добавить метод на получение всех существующих комнат

        /// <summary>
        /// Добавление комнаты
        /// </summary>
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Add([FromBody] AddRoomRequest request)
        {
            var existingRoom = await _repository.GetRoomByName(request.Name);
            if (existingRoom == null)
            {
                var newRoom = _mapper.Map<AddRoomRequest, Room>(request);
                await _repository.AddRoom(newRoom);
                return StatusCode(201, $"Комната {request.Name} добавлена!");
            }

            return StatusCode(409, $"Ошибка: Комната {request.Name} уже существует.");
        }

        /// <summary>
        /// Частично обновить комнату (меняются только переданные поля).
        /// </summary>
        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> Putch(Guid id, [FromBody] RoomPatchRequest dto)
        {
            if (dto == null) return BadRequest("Тело запроса пустое.");

            var updated = await _repository.PutchRoom(id, dto);
            if (updated == null)
                return NotFound($"Комната с id {id} не найдена."); // соответствующая ошибка

            // Успех — возвращаем сообщение и обновлённую сущность (или просто сообщение если хотите)
            return Ok($"Комната '{updated.Name}' успешно обновлена.");

        }
    }
}