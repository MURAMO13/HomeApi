using HomeApi.Contracts.Models.Rooms;
using HomeApi.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HomeApi.Data.Repos
{
    /// <summary>
    /// Репозиторий для операций с объектами типа "Room" в базе
    /// </summary>
    public class RoomRepository : IRoomRepository
    {
        private readonly HomeApiContext _context;
        
        public RoomRepository (HomeApiContext context)
        {
            _context = context;
        }
        
        /// <summary>
        ///  Найти комнату по имени
        /// </summary>
        public async Task<Room> GetRoomByName(string name)
        {
            return await _context.Rooms.Where(r => r.Name == name).FirstOrDefaultAsync();
        }
        
        /// <summary>
        ///  Добавить новую комнату
        /// </summary>
        public async Task AddRoom(Room room)
        {
            var entry = _context.Entry(room);
            if (entry.State == EntityState.Detached)
                await _context.Rooms.AddAsync(room);
            
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Обновить комнату: найти по id
        /// </summary>
        public async Task<Room?> PutchRoom(Guid id, RoomPatchRequest dto)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
            if (room == null)
                return null;

            if (dto.Name is not null) room.Name = dto.Name;
            if (dto.Area.HasValue) room.Area = dto.Area.Value;
            if (dto.Voltage.HasValue) room.Voltage = dto.Voltage.Value;
            if (dto.GasConnected.HasValue) room.GasConnected = dto.GasConnected.Value;

            await _context.SaveChangesAsync();

            return room;
        }
    }
}