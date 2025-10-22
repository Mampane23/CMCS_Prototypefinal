using Xunit;
using CMCS.Models;

namespace CMCS.Tests
{
    public class ClaimTests
    {
        [Fact]
        public void TotalAmount_ShouldBeCalculatedCorrectly()
        {
            // Arrange
            var claim = new Claim
            {
                LecturerName = "John Doe",
                HoursWorked = 10,
                HourlyRate = 200
            };

            // Act
            var total = claim.TotalAmount;

            // Assert
            Assert.Equal(2000, total);
        }

        [Fact]
        public void DefaultStatus_ShouldBePending()
        {
            // Arrange
            var claim = new Claim
            {
                LecturerName = "Jane Smith",
                HoursWorked = 5,
                HourlyRate = 100
            };

            // Assert
            Assert.Equal("Pending", claim.Status);
        }

        [Fact]
        public void LecturerName_ShouldBeSetCorrectly()
        {
            // Arrange
            var claim = new Claim
            {
                LecturerName = "Dr. Kabelo",
                HoursWorked = 8,
                HourlyRate = 150
            };

            // Assert
            Assert.Equal("Dr. Kabelo", claim.LecturerName);
        }

        [Fact]
        public void UploadedFiles_CanBeNull()
        {
            // Arrange
            var claim = new Claim
            {
                LecturerName = "Lerato",
                HoursWorked = 4,
                HourlyRate = 250,
                UploadedFiles = null
            };

            // Assert
            Assert.Null(claim.UploadedFiles);
        }

        [Fact]
        public void Status_CanBeUpdated()
        {
            // Arrange
            var claim = new Claim
            {
                LecturerName = "Mpho",
                HoursWorked = 3,
                HourlyRate = 100
            };

            // Act
            claim.Status = "Approved";

            // Assert
            Assert.Equal("Approved", claim.Status);
        }
    }
}