﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using LogicLayer;
using DataAccessFakes;
using DataAccessInterfaces;
using DataObjects;
using System.Collections.Generic;

namespace LogicLayerTests
{
    [TestClass]
    public class UserManagerTests
    {
        IUserManager _userManager = null;

        [TestInitialize]
        public void TestSetUp()
        {
            _userManager = new UserManager(new UserAccessorFake());
        }
        [TestMethod]
        public void TestHashSha256ReturnsACorrectHashValue()
        {
            // in TDD (test-driven development, the test comes first)
            // we use the red-green-refactor workflow
            // we write the test method with the A-A-A framework

            // Arrange - set up the test condition
            string testString = "newuser";
            string actualHash = "";
            string expectedHash = "9c9064c59f1ffa2e174ee754d2979be80dd30db552ec03e7e327e9b1a4bd594e";

            // Act - invoke the method being tested, and capture results
            actualHash = _userManager.HashSha256(testString);

            // Assert
            Assert.AreEqual(expectedHash, actualHash);
        }
        [TestMethod]
        public void TestAuthenticateUserPassesWithCorrectEmailAndPassword()
        {
            // arrange 
            string email = "Liam@gmail.com";
            string password = "password";
            bool expectedResult = true;
            bool actualResult = false;

            // act
            actualResult = _userManager.AuthenticateUser(email, password);

            // assert 
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestAuthenticateUserFailsWithBadEmailAndPassword()
        {
            // arrange 
            string email = "tess@company.com";
            string password = "newloser";
            bool expectedResult = false;
            bool actualResult = true;

            // act
            actualResult = _userManager.AuthenticateUser(email, password);

            // assert 
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestGetUserByEmailReturnsCorrectUser()
        {
            // arrange
            string email = "Liam@gmail.com";
            int expectedUserID = 4;
            int actualUserID = 0;

            // act
            User user = _userManager.GetUserVMByEmail(email);
            actualUserID = user.UserID;

            // Assert
            Assert.AreEqual(expectedUserID, actualUserID);
        }
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestGetUserByEmailFailsWithBadEmail()
        {
            // arrange
            string email = "ness@company.com"; // bad email
            int expectedUserID = 1;
            int actualUserID = 0;

            // act
            User user = _userManager.GetUserVMByEmail(email);
            actualUserID = user.UserID;

            // assert - nothing to do

        }
        [TestMethod]
        public void TestGetRolesByUserIdReturnsCorrectRoles()
        {
            // arrange
            int testID = 4;
            int expectedRoleCount = 2;
            int actualRoleCount = 0;

            // act
            actualRoleCount = _userManager.GetRolesByUserID(testID).Count;


            // assert
            Assert.AreEqual(expectedRoleCount, actualRoleCount);
        }
        [TestMethod]
        public void TestResetPasswordWorksCorrectly()
        {
            // arrange
            string email = "Liam@gmail.com";
            string password = "password";
            string newPassword = "P@ssw0rd";
            bool expectedResult = true;
            bool actualResult = false;

            // act
            actualResult = _userManager.ResetPassword(email, password, newPassword);

            // assert
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestSelectAllUsersWorksCorrectly()
        {
            // arrange

            int expectedNumberOfUsers = 3;
            int actualNumberOfUsers = 0;

            // act
            actualNumberOfUsers = _userManager.SelectAllUsers().Count;

            // assert
            Assert.AreEqual(expectedNumberOfUsers, actualNumberOfUsers);
        }
        [TestMethod]
        public void TestInsertUserWorksCorrectly()
        {
            // arrange

            User user = new User()
            {
                UserID = 1,
                ProfileName = "Easton68",
                Email = "LiamEastonNewUserInsertTest@gmail.com",
                FirstName = "Liam",
                LastName = "Easton",
                ImageFilePath = "muse.png"
            };

            string password = "password";

            bool expectedResult = true;
            bool actualResult = false;

            // act
            actualResult = _userManager.InsertUser(user, password);

            // assert
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestInsertUserFailsWithAlreadyExistingEmail()
        {
            // arrange

            User user = new User()
            {
                UserID = 1,
                ProfileName = "Easton68",
                Email = "Liam@gmail.com",
                FirstName = "Liam",
                LastName = "Easton",
                ImageFilePath = "muse.png"
            };

            string password = "password";

            bool expectedResult = true;
            bool actualResult = false;

            // act
            actualResult = _userManager.InsertUser(user, password);

            // assert
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void TestInsertUserFailsWithAlreadyExistingProfileName()
        {
            // arrange

            User user = new User()
            {
                UserID = 1,
                ProfileName = "Easton67",
                Email = "LiamNewUserEmail@gmail.com",
                FirstName = "Liam",
                LastName = "Easton",
                ImageFilePath = "muse.png"
            };

            string password = "password";

            bool expectedResult = true;
            bool actualResult = false;

            // act
            actualResult = _userManager.InsertUser(user, password);

            // assert
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestUpdateMinutesListenedWorksCorrectly()
        {
            // arrange
            int userID = 3;
            int newMinutesListened = 90433;

            bool expectedResult = true;
            bool actualResult = false;

            // act
            actualResult = _userManager.UpdateMinutesListened(userID, newMinutesListened);

            // assert
            Assert.AreEqual(expectedResult, actualResult);
        }
        [TestMethod]
        public void TestUpdateActiveWorksCorrectly()
        {
            // arrange
            int userID = 3;
            bool oldActive = true;
            bool newActive = false;

            bool expectedResult = true;
            bool actualResult = false;

            // act
            actualResult = _userManager.UpdateActiveUser(userID, oldActive, newActive);

            // assert
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
