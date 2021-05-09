using GenesisConsult.Nightclub.Application.API.Controllers;
using GenesisConsult.Nightclub.Application.Core.Mapper;
using GenesisConsult.Nightclub.Application.Core.Models;
using GenesisConsult.Nightclub.Application.Core.Models.Dto;
using GenesisConsult.Nightclub.Application.Core.Services.Database.Interfaces;
using GenesisConsult.Nightclub.Infrastructure.Implementations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenesisConsult.Nightclub.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private MembersController controller;
        private MemberDto defaultMemberDto;
        private Member defaultMember;
        private Mock<IMemberDataAccess> mockContext;

        [TestInitialize]
        public void Initialize()
        {
            defaultMemberDto = new MemberDto()
            {
                IdentityCard = new IdentityCardDto()
                {
                    CardNumber = 1,
                    Lastname = "test",
                    Firstname = "test",
                    Birthdate = new DateTime(1990, 10, 10),
                    NationalNumber = "888.55.55-175-25",
                    ValidFrom = new DateTime(1990, 10, 10),
                    ValidTo = new DateTime(3000, 10, 10)
                },
                MemberCard = new MemberCardDto()
                {
                    Id = 1
                },
                ContactDetails = "test@test.com"
            };

            mockContext = new Mock<IMemberDataAccess>();

            var identityCardMapper = new IdentityCardMapper();
            var memberCardMapper = new MemberCardMapper();
            var memberMapper = new MemberMapper(identityCardMapper, memberCardMapper);

            defaultMember = memberMapper.MapDTOToModel(defaultMemberDto);
            defaultMember.Id = 1;

            mockContext.Setup(p => p.GetMemberAsync(1)).ReturnsAsync(defaultMember);
            mockContext.Setup(p => p.GetMemberAsync("888.55.55-175-25")).ReturnsAsync(defaultMember);
            var memberTwo = defaultMember.Clone();
            memberTwo.Id = 2;
            memberTwo.IdentityCard.CardNumber = 2;
            memberTwo.MemberCard.Id = 2;
            memberTwo.IdentityCard.NationalNumber = "888.55.55-175-26";
            mockContext.Setup(p => p.GetMemberAsync(2)).ReturnsAsync(memberTwo);
            mockContext.Setup(p => p.GetMemberAsync("888.55.55-175-26")).ReturnsAsync(memberTwo);
            mockContext.Setup(p => p.GetMembersAsync()).ReturnsAsync(new List<Member>()
            {
                defaultMember,
                memberTwo
            });

            controller = new MembersController(mockContext.Object, memberMapper);
        }

        [TestMethod]
        public async Task TestRegisterMember()
        {
            var memberDtoClone = defaultMemberDto.Clone();
            memberDtoClone.IdentityCard.CardNumber = 3;
            memberDtoClone.MemberCard.Id = 3;
            memberDtoClone.IdentityCard.NationalNumber = "888.55.55-175-27";
            ActionResult<Member> response = await controller.RegisterMember(memberDtoClone);
            var result = response.Result as CreatedAtActionResult;

            Assert.AreEqual(201, result?.StatusCode);
        }

        [TestMethod]
        public async Task TestRegisterMemberEmpty()
        {
            var memberDtoClone = defaultMemberDto.Clone();
            memberDtoClone.ContactDetails = null;
            ActionResult<Member> response = await controller.RegisterMember(memberDtoClone);
            var result = response.Result as BadRequestObjectResult;

            Assert.AreEqual(400, result?.StatusCode);
        }

        [TestMethod]
        public async Task TestRegisterMemberSameNationalNumber()
        {
            var memberDtoClone = defaultMemberDto.Clone();
            memberDtoClone.IdentityCard.CardNumber = 3;
            memberDtoClone.MemberCard.Id = 3;
            ActionResult<Member> response = await controller.RegisterMember(memberDtoClone);
            var result = response.Result as BadRequestObjectResult;

            Assert.AreEqual(400, result?.StatusCode);
        }

        [TestMethod]
        public async Task TestRegisterMemberAgain()
        {
            mockContext.Setup(p => p.GetMembersAsync(It.IsAny<Member>(), false)).ReturnsAsync(new List<Member>()
            {
                defaultMember
            });

            var memberDtoClone = defaultMemberDto.Clone();
            memberDtoClone.MemberCard.Id = 15;
            ActionResult<Member> response = await controller.RegisterMember(memberDtoClone);
            var result = response.Result as CreatedAtActionResult;

            Assert.AreEqual(201, result?.StatusCode);
        }

        [TestMethod]
        public async Task TestRegisterMemberAgainInvalidId()
        {
            var memberDtoClone = defaultMemberDto.Clone();
            memberDtoClone.IdentityCard.CardNumber = 3;
            memberDtoClone.MemberCard.Id = 2;
            ActionResult<Member> response = await controller.RegisterMember(memberDtoClone);
            var result = response.Result as BadRequestObjectResult;

            Assert.AreEqual(400, result?.StatusCode);
        }

        [TestMethod]
        public async Task TestRegisterMemberInvalidNationalNumber()
        {
            var memberDtoClone = defaultMemberDto.Clone();
            memberDtoClone.IdentityCard.NationalNumber = "4584";
            ActionResult<Member> response = await controller.RegisterMember(memberDtoClone);
            var result = response.Result as BadRequestObjectResult;

            Assert.AreEqual(400, result?.StatusCode);
        }

        [TestMethod]
        public async Task TestUpdateMember()
        {
            var response = (await controller.UpdateMember(1, defaultMemberDto)) as NoContentResult;

            Assert.AreEqual(204, response?.StatusCode);
        }

        [TestMethod]
        public async Task TestUpdateMemberInvalidField()
        {
            var memberDtoClone = defaultMemberDto.Clone();
            memberDtoClone.IdentityCard.Lastname = null;
            var response = (await controller.UpdateMember(1, memberDtoClone)) as BadRequestObjectResult;

            Assert.AreEqual(400, response?.StatusCode);
        }

        [TestMethod]
        public async Task TestDeleteMember()
        {
            var response = (await controller.DeleteMember(1)) as NoContentResult;

            Assert.AreEqual(204, response?.StatusCode);
        }

        [TestMethod]
        public async Task TestDeleteMemberNotFound()
        {
            var response = (await controller.DeleteMember(999)) as NotFoundResult;

            Assert.AreEqual(404, response?.StatusCode);
        }
    }
}
