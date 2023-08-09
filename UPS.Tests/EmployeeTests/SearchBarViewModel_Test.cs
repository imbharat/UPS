using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Employee.ViewModels;
using System.Windows.Input;

namespace UPS.Tests.EmployeeTests
{
    [TestFixture]
    public class SearchBarViewModelTest
    {
        [Test]
        public void SearchCommand_EmptyFields_DoesNotInvokeSearchEvent()
        {
            // Arrange
            var viewModel = new SearchBarViewModel();
            bool searchEventInvoked = false;
            viewModel.OnEmployeeSearch += (sender, searchString) => searchEventInvoked = true;

            // Act
            ICommand searchCommand = viewModel.SearchCommand;
            searchCommand.Execute(null);

            // Assert
            Assert.IsFalse(searchEventInvoked);
        }

        [Test]
        public void SearchCommand_NonEmptyFields_InvokesSearchEventWithCorrectQueryString()
        {
            // Arrange
            var viewModel = new SearchBarViewModel();
            string searchString = null;
            viewModel.OnEmployeeSearch += (sender, query) => searchString = query;
            viewModel.SearchId = "1";
            viewModel.SearchName = "John";
            viewModel.SearchEmail = "john@example.com";
            viewModel.SearchGender = "Male";
            viewModel.SearchStatus = "Active";

            // Act
            ICommand searchCommand = viewModel.SearchCommand;
            searchCommand.Execute(null);

            // Assert
            Assert.AreEqual("&id=1&name=John&email=john@example.com&gender=Male&status=Active", searchString);
        }

        [Test]
        public void ClearCommand_InvokesClearEventAndResetsFields()
        {
            // Arrange
            var viewModel = new SearchBarViewModel();
            bool clearEventInvoked = false;
            viewModel.OnClearSearch += (sender, args) => clearEventInvoked = true;
            viewModel.SearchId = "1";
            viewModel.SearchName = "John";
            viewModel.SearchEmail = "john@example.com";
            viewModel.SearchGender = "Male";
            viewModel.SearchStatus = "Active";

            // Act
            ICommand clearCommand = viewModel.ClearCommand;
            clearCommand.Execute(null);

            // Assert
            Assert.IsTrue(clearEventInvoked);
            Assert.AreEqual(string.Empty, viewModel.SearchId);
            Assert.AreEqual(string.Empty, viewModel.SearchName);
            Assert.AreEqual(string.Empty, viewModel.SearchEmail);
            Assert.AreEqual(null, viewModel.SearchGender);
            Assert.AreEqual(null, viewModel.SearchStatus);
        }
    }
}
