using Employee.Models;
using Employee.Services;
using Employee.ViewModels;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UPS.Tests.EmployeeTests
{
    [TestFixture]
    public class EmployeeViewModelTests
    {
        [Test]
        public async Task LoadEmployeesAsync_CallsGetData_PopulateGrid()
        {
            // Arrange
            var employeeServiceMock = new Mock<IEmployeeService>();
            var addEditEmployeeViewModelMock = new Mock<AddEditEmployeeViewModel>();
            var searchBarViewModelMock = new Mock<SearchBarViewModel>();
            var viewModel = new EmployeeViewModel(employeeServiceMock.Object, addEditEmployeeViewModelMock.Object, searchBarViewModelMock.Object);

            // Set up the mock to return a list of employees when GetEmployeesAsync is called
            employeeServiceMock.Setup(mock => mock.GetEmployeesAsync("", "?page=1"))
                .ReturnsAsync(new List<EmployeeModel>());

            // Act
            await viewModel.LoadEmployeesAsync();

            // Assert
            employeeServiceMock.Verify(mock => mock.GetEmployeesAsync("", "?page=1"), Times.Once);
        }

        [Test]
        public void NextPageCommand_InvokesLoadNextPage()
        {
            // Arrange
            var employeeServiceMock = new Mock<IEmployeeService>();
            var addEditEmployeeViewModelMock = new Mock<AddEditEmployeeViewModel>();
            var searchBarViewModelMock = new Mock<SearchBarViewModel>();
            var viewModel = new EmployeeViewModel(employeeServiceMock.Object, addEditEmployeeViewModelMock.Object, searchBarViewModelMock.Object);
            employeeServiceMock.Setup(mock => mock.GetEmployeesAsync("", "?page=2")).ReturnsAsync(new List<EmployeeModel>());

            // Act
            ICommand nextPageCommand = viewModel.NextPageCommand;
            nextPageCommand.Execute(null);

            // Assert
            employeeServiceMock.Verify(mock => mock.GetEmployeesAsync("", "?page=2"), Times.Once);
        }

        [Test]
        public void PreviousPageCommand_InvokesLoadPreviousPage()
        {
            // Arrange
            var employeeServiceMock = new Mock<IEmployeeService>();
            var addEditEmployeeViewModelMock = new Mock<AddEditEmployeeViewModel>();
            var searchBarViewModelMock = new Mock<SearchBarViewModel>();
            var viewModel = new EmployeeViewModel(employeeServiceMock.Object, addEditEmployeeViewModelMock.Object, searchBarViewModelMock.Object);
            employeeServiceMock.Setup(mock => mock.GetEmployeesAsync("", "?page=0")).ReturnsAsync(new List<EmployeeModel>());

            // Act
            ICommand previousPageCommand = viewModel.PreviousPageCommand;
            previousPageCommand.Execute(null);

            // Assert
            employeeServiceMock.Verify(mock => mock.GetEmployeesAsync("", "?page=0"), Times.Once);
        }
    }
}
