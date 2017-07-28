using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProfileService.Model;
using ProfileService.Model.BindingModel;
using ProfileService.Model.ViewModel;

namespace ProfileService.Adapter
{
    public static class PositionAdapter
    {
        public static Position ToModel(PositionBindingModel bindingModel)
        {
            if (bindingModel == null)
                return null;
            var model = new Position();
            model.PositionName = bindingModel.Name; 
            model.PositionId = bindingModel.Id;
            return model;
        }
        public static PositionViewModel ToViewModel(Position position)
        {
            var ViewModel = new PositionViewModel();
            ViewModel.Id = position.PositionId;
            ViewModel.Name = position.PositionName;
            return ViewModel;
        }
    }
}
