using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProfileService.Adapter;
using ProfileService.IServiceInterfaces;
using ProfileService.Model;
using ProfileService.Model.BindingModel;
using ProfileService.Model.ViewModel;

namespace ProfileService.Controllers
{
     
    [Route("api/Position")]
    public class PositionController : Controller
    {
        private ITeamService _teamService;
        private IPositionService _positionService;
        public PositionController(ITeamService teamService, IPositionService positionService)
        {
            _teamService = teamService;
            _positionService = positionService;
        }
        // GET: api/Profile
        [HttpGet]
        [Route("PositionList")]
        public async Task<IEnumerable<PositionViewModel>> GetAllPosition()
        {
            var PositionViewModelList = new List<PositionViewModel>();
            foreach(Position pos in await _positionService.GetAllAsync())
            {
                PositionViewModelList.Add(PositionAdapter.ToViewModel(pos));
            }
            return PositionViewModelList;
        }

        // GET: api/Profile/5
        [HttpGet]
        [Route("Position/{PositionId}")]
        public async Task<PositionViewModel> GetPosition(int PositionId)
        {
            return PositionAdapter.ToViewModel(await _positionService.GetAsync(PositionId));
        }
        
        // POST: api/Profile
        [HttpPost("Create")]
        public async Task<PositionBindingModel> CreatePosition([FromBody]PositionBindingModel newPosition)
        {
            await _positionService.CreateAsync(PositionAdapter.ToModel(newPosition));
            return newPosition;
        }
        
        // PUT: api/Profile/5
        [HttpPut]
        [Route("UpdateInfo")]
        public async Task<PositionBindingModel> UpdatePosition([FromBody]PositionBindingModel updatePosition)
        { 
            
            await _positionService.UpdateAsync(PositionAdapter.ToModel(updatePosition));
            return updatePosition;
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete]
        [Route("Delete/{PositionId}")]
        public async Task<int> Delete(int PositionId)
        {
            return await _positionService.RemovePositionAsync(PositionId);
        }
          
    }
}
