using HeatEnergyConsumption.ViewModels.SortStates;

namespace HeatEnergyConsumption.ViewModels.SortViewModels
{
    public class ViolatorsProductsTypesSortViewModel
    {
        public ViolatorsProductsTypesSortViewModel() { }

        public ViolatorsProductsTypesSortViewModel(ViolatorsProductsTypesSortState sortOrder)
        {
            CurrentOrder = sortOrder;

            CodeOrder = sortOrder == ViolatorsProductsTypesSortState.CodeAsc ?
                ViolatorsProductsTypesSortState.CodeDesc : ViolatorsProductsTypesSortState.CodeAsc;
            TypeOrder = sortOrder == ViolatorsProductsTypesSortState.TypeAsc ?
                ViolatorsProductsTypesSortState.TypeDesc : ViolatorsProductsTypesSortState.TypeAsc;
            OrganizationOrder = sortOrder == ViolatorsProductsTypesSortState.OrganizationAsc ?
                ViolatorsProductsTypesSortState.OrganizationDesc : ViolatorsProductsTypesSortState.OrganizationAsc;
            ExceedingOrder = sortOrder == ViolatorsProductsTypesSortState.ExceedingAsc ?
                ViolatorsProductsTypesSortState.ExceedingDesc : ViolatorsProductsTypesSortState.ExceedingAsc;
            YearOrder = sortOrder == ViolatorsProductsTypesSortState.YearAsc ?
                ViolatorsProductsTypesSortState.YearDesc : ViolatorsProductsTypesSortState.YearAsc;
        }

        public ViolatorsProductsTypesSortState CurrentOrder {  get; set; }

        public ViolatorsProductsTypesSortState CodeOrder {  get; set; }

        public ViolatorsProductsTypesSortState TypeOrder {  get; set; }

        public ViolatorsProductsTypesSortState OrganizationOrder {  get; set; }

        public ViolatorsProductsTypesSortState ExceedingOrder { get; set; }

        public ViolatorsProductsTypesSortState YearOrder {  get; set; }
    }
}