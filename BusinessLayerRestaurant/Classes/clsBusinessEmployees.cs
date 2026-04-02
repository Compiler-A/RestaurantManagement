using DataLayerRestaurant;
using RestaurantDataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace BusinessLayerRestaurant
{
    public class clsEmployeesDtoContainer : IDTOBEmployees
    {
        private DTOEmployeesCRequest? _CRequest;
        public DTOEmployeesCRequest? CreateRequest
        {
            get => _CRequest;
            set => _CRequest = value;
        }
        private DTOEmployeesURequest? _URequest;
        public DTOEmployeesURequest? UpdateRequest
        {
            get => _URequest;
            set => _URequest = value;
        }
    }

    public class clsEmployeesRepositoryBridge : IInterfaceBEmployees
    {
        private IDataEmployees _IEmployees;
        public IDataEmployees IData
        {
            get => _IEmployees;
            set => _IEmployees = value;
        }

        private IBusinessJobRoles _IJobRoles;
        public IBusinessJobRoles IBusinessJobRole
        {
            get => _IJobRoles;
            set => _IJobRoles = value;
        }

        public clsEmployeesRepositoryBridge(IDataEmployees Employee, IBusinessJobRoles JobRoles)
        {
            _IEmployees = Employee;
            _IJobRoles = JobRoles;
        }
    }

    public class clsJobRoleLoader : ICompositionBEmployees
    {
        private IBusinessJobRoles _IData;

        public clsJobRoleLoader(IBusinessJobRoles JobRole)
        {
            _IData = JobRole;
        }

        public async Task LoadDataAsync(DTOEmployees item)
        {
            item.JobRoles = await _IData.GetJobRoleAsync(item.JobID);
        }

    }

    public class clsCompositionEmployeeesLoader: ICompositionBEmployees
    {
        private IEnumerable<ICompositionBEmployees> _loaders;
        public clsCompositionEmployeeesLoader
            (IEnumerable<ICompositionBEmployees> loaders)
        {
            _loaders = loaders;
        }
        public async Task LoadDataAsync(DTOEmployees item)
        {
            foreach (var item1 in _loaders)
            {
                await item1.LoadDataAsync(item);
            }
        }
    }

    public class clsEmployeesReader :  clsCompositionEmployeeesLoader ,IReadableBEmployees
    {
        private IInterfaceBEmployees _Interface;
        public clsEmployeesReader(IInterfaceBEmployees Interface, IEnumerable<ICompositionBEmployees> Loaders) 
            : base(Loaders)
        {
            _Interface = Interface;
        }
        public async Task<DTOEmployees?> LoginAsync(DTOEmployeesLoginRequest Request)
        {
            var dto =  await _Interface.IData.GetLoginEmployeeAsync(Request);

            if (dto == null)
            {
                return null;
            }
            await LoadDataAsync(dto);
            return dto;
        }

        public async Task<DTOEmployees?> GetAsync(int ID)
        {
            var dto  = await _Interface.IData.GetEmployeeAsync(ID);
            if (dto == null)
            {
                return null;
            }
            await LoadDataAsync(dto);
            return dto;
        }

        public async Task<DTOEmployees?> GetAsync(string UserName)
        {
            var dto = await _Interface.IData.GetEmployeeAsync(UserName);
            if (dto == null)
            {
                return null;
            }
            await LoadDataAsync(dto);
            return dto;
        }
        public async Task<List<DTOEmployees>> GetAllAsync(int page)
        {
            var dto = await _Interface.IData.GetAllEmployeesAsync(page);
            foreach (var item in dto)
            {
                await LoadDataAsync(item);
            }
            return dto;
        }

    }

    public class clsEmployeesWriter : clsCompositionEmployeeesLoader , IWritableBEmployees
    {
        private IInterfaceBEmployees _Interface;
        public clsEmployeesWriter(IInterfaceBEmployees Interface, IEnumerable<ICompositionBEmployees> Loaders)
            : base(Loaders)
        {
            _Interface = Interface;
        }

        public async Task<bool> ChangePasswordAsync(DTOEmployeesChangedPassword Request)
        {
            return await _Interface.IData.ChangedPasswordEmployeeAsync(Request);
        }

        public async Task<DTOEmployees?> CreateAsync(DTOEmployeesCRequest Request)
        {
            if (Request == null)
            { return null; }
            var dto = await _Interface.IData.AddEmployeeAsync(Request);
            if (dto != null)
            {
                await LoadDataAsync(dto);
                return dto;
            }
            return null;
        }

        public async Task<DTOEmployees?> UpdateAsync(DTOEmployeesURequest Request)
        {
            if (Request == null)
            { return null; }
            var dto = await _Interface.IData.UpdateEmployeeAsync(Request);
            if (dto != null)
            {
                await LoadDataAsync(dto);
                return dto;
            }
            return null;
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            return await _Interface.IData.DeleteEmployeeAsync(ID);
        }
    }

    public class clsBusinessEmployees : IBusinessEmployees
    {
        IReadableBEmployees _IRead;
        IWritableBEmployees _IWrite;
        IDTOBEmployees _IDTO;
        IInterfaceBEmployees _Interface;

        public clsBusinessEmployees
            (IReadableBEmployees Read, IWritableBEmployees Write, IDTOBEmployees DTO, IInterfaceBEmployees Interface)
        {
            _IDTO = DTO;
            _IRead = Read;
            _IWrite = Write;
            _Interface = Interface;
        }

        public IBusinessJobRoles IJobRole
        {
            get => _Interface.IBusinessJobRole;
            set => _Interface.IBusinessJobRole = value;
        }

        public DTOEmployeesCRequest? CreateRequest
        {
            get => _IDTO.CreateRequest;
            set => _IDTO.CreateRequest = value;
        }

        public DTOEmployeesURequest? UpdateRequest
        {
            get => _IDTO.UpdateRequest;
            set => _IDTO.UpdateRequest = value;
        }


        public async Task<DTOEmployees?> GetLoginEmployeeAsync(DTOEmployeesLoginRequest request)
        {
            return await _IRead.LoginAsync(request);
        }

        public async Task<bool> ChangePasswordAsync(DTOEmployeesChangedPassword Request)
        {
            return await _IWrite.ChangePasswordAsync(Request);
        }
        

        public async Task<DTOEmployees?> GetEmployeeAsync(int ID)
        {
            return await _IRead.GetAsync(ID);
        }

        public async Task<DTOEmployees?> GetEmployeeAsync(string Name)
        {
            return await _IRead.GetAsync(Name);
        }

        public async Task<List<DTOEmployees>> GetAllEmployeesAsync(int Page)
        {
            return await _IRead.GetAllAsync(Page);
        }

        public async Task<DTOEmployees?> CreateEmployeeAsync(DTOEmployeesCRequest request)
        {
            return await _IWrite.CreateAsync(request);
        }

        public async Task<DTOEmployees?> UpdateEmployeeAsync(DTOEmployeesURequest request)
        {
            return await _IWrite.UpdateAsync(request);
        }

        public async Task<bool> DeleteEmployeeAsync(int ID)
        {
            return await _IWrite.DeleteAsync(ID);
        }
    }
}
