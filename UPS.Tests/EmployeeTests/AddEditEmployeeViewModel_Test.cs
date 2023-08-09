using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Employee.ViewModels;
using Employee.Models;
using System.Windows.Input;

namespace UPS.Tests.EmployeeTests
{
    [TestFixture]
    public class AddEditEmployeeViewModelTests
    {
        [Test]
        public void SetValues_WithEmployee_SetsProperties()
        {
            // Arrange
            var viewModel = new AddEditEmployeeViewModel();
            var employee = new EmployeeModel
            {
                id = 1,
                name = "John Doe",
                email = "john@example.com",
                gender = "Male",
                status = "Active"
            };

            // Act
            viewModel.SetValues(employee);

            // Assert
            Assert.AreEqual(1, viewModel.Id);
            Assert.AreEqual("John Doe", viewModel.Name);
            Assert.AreEqual("john@example.com", viewModel.Email);
            Assert.AreEqual("Male", viewModel.Gender);
            Assert.AreEqual("Active", viewModel.Status);
        }

        [Test]
        public void SubmitCommand_MissingValues_DoesNotInvokeOnSubmitEvent()
        {
            // Arrange
            var viewModel = new AddEditEmployeeViewModel();
            bool eventInvoked = false;
            viewModel.OnSubmit += (sender, employee) => eventInvoked = false;

            // Act
            ICommand submitCommand = viewModel.SubmitCommand;
            viewModel.Name = "John Doe"; //only Name provided, other values are null
            submitCommand.Execute(null);

            // Assert
            Assert.IsFalse(eventInvoked);
        }

        [Test]
        public void CancelCommand_InvokesOnCancelEvent()
        {
            // Arrange
            var viewModel = new AddEditEmployeeViewModel();
            bool eventInvoked = false;
            viewModel.OnCancel += (sender, args) => eventInvoked = true;

            // Act
            ICommand cancelCommand = viewModel.CancelCommand;
            cancelCommand.Execute(null);

            // Assert
            Assert.IsTrue(eventInvoked);
        }

        [Test]
        public void SetValues_NullEmployee_DoesNotThrowException()
        {
            // Arrange
            var viewModel = new AddEditEmployeeViewModel();

            // Act and Assert
            Assert.DoesNotThrow(() => viewModel.SetValues(null));
        }
    }
}
