using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using JHipsterNetSampleApplication.Data;
using JHipsterNetSampleApplication.Models;
using JHipsterNetSampleApplication.Test.Setup;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Xunit;

namespace JHipsterNetSampleApplication.Test.Web.Rest {
    public class LabelResourceIntTest {
        public LabelResourceIntTest()
        {
            _factory = new NhipsterWebApplicationFactory<Startup>();
            _client = _factory.CreateClient();

            _applicationDatabaseContext = _factory.GetRequiredService<ApplicationDatabaseContext>();

            InitTest();
        }

        private const string DefaultName = "AAAAAAAAAA";
        private const string UpdatedName = "BBBBBBBBBB";

        private readonly NhipsterWebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;

        private readonly ApplicationDatabaseContext _applicationDatabaseContext;

        private Label _label;

        private Label CreateEntity()
        {
            return new Label {
                Name = DefaultName
            };
        }

        private void InitTest()
        {
            _label = CreateEntity();
        }

        [Fact]
        public async Task CheckNameIsRequired()
        {
            var databaseSizeBeforeTest = _applicationDatabaseContext.Labels.Count();

            // set the field null
            _label.Name = null;

            // Create the Label, which fails.
            var response = await _client.PostAsync("/api/labels", TestUtil.ToJsonContent(_label));
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var labelList = _applicationDatabaseContext.Labels.ToList();
            labelList.Count().Should().Be(databaseSizeBeforeTest);
        }

        [Fact]
        public async Task CreateLabel()
        {
            var databaseSizeBeforeCreate = _applicationDatabaseContext.Labels.Count();

            // Create the Label
            var response = await _client.PostAsync("/api/labels", TestUtil.ToJsonContent(_label));
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            // Validate the Label in the database
            var labelList = _applicationDatabaseContext.Labels.ToList();
            labelList.Count().Should().Be(databaseSizeBeforeCreate + 1);
            var testLabel = labelList[labelList.Count - 1];
            testLabel.Name.Should().Be(DefaultName);
        }

        [Fact]
        public async Task CreateLabelWithExistingId()
        {
            var databaseSizeBeforeCreate = _applicationDatabaseContext.Labels.Count();

            // Create the Label with an existing ID
            _label.Id = 1L;

            // An entity with an existing ID cannot be created, so this API call must fail
            var response = await _client.PostAsync("/api/labels", TestUtil.ToJsonContent(_label));
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // Validate the Label in the database
            var labelList = _applicationDatabaseContext.Labels.ToList();
            labelList.Count().Should().Be(databaseSizeBeforeCreate);
        }

        [Fact]
        public async Task CreateLabelWithManyToManyAssociation()
        {
            // Due to JsonIgnore annotation on Operations property, we first create the Label
            // This allows the operation to create the relationship
            var databaseSizeBeforeCreate = _applicationDatabaseContext.Labels.Count();

            // Create the Label
            var response = await _client.PostAsync("/api/labels", TestUtil.ToJsonContent(_label));
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var testLabel = _applicationDatabaseContext.Labels.ToList()[0];

            // Create an Operation to test the ManyToMany association
            var operation = new Operation {
                Date = DateTime.UnixEpoch,
                Description = "AAAAAAAAAA",
                Amount = new decimal(1.0)
            };
            operation.Labels.Add(testLabel);
            _applicationDatabaseContext.Operations.Add(operation);
            await _applicationDatabaseContext.SaveChangesAsync();
            operation = _applicationDatabaseContext.Operations.ToList()[0];

            // Validate the Label in the database
            var labelList = _applicationDatabaseContext.Labels
                .Include("OperationLabels.Operation")
                .ToList();
            labelList.Count().Should().Be(databaseSizeBeforeCreate + 1);
            testLabel = labelList[labelList.Count - 1];
            testLabel.Name.Should().Be(DefaultName);
            testLabel.Operations[0].Should().Be(operation);
        }

        [Fact]
        public async Task DeleteLabel()
        {
            // Initialize the database
            _applicationDatabaseContext.Labels.Add(_label);
            await _applicationDatabaseContext.SaveChangesAsync();

            var databaseSizeBeforeDelete = _applicationDatabaseContext.Labels.Count();

            var response = await _client.DeleteAsync($"/api/labels/{_label.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Validate the database is empty
            var labelList = _applicationDatabaseContext.Labels.ToList();
            labelList.Count().Should().Be(databaseSizeBeforeDelete - 1);
        }

        [Fact]
        public void EqualsVerifier()
        {
            TestUtil.EqualsVerifier(typeof(Label));
            var label1 = new User {
                Id = "label-1"
            };
            var label2 = new User {
                Id = label1.Id
            };
            label1.Should().Be(label2);
            label2.Id = "label-2";
            label1.Should().NotBe(label2);
            label1.Id = null;
            label1.Should().NotBe(label2);
        }

        [Fact]
        public async Task GetAllLabels()
        {
            // Initialize the database
            _applicationDatabaseContext.Labels.Add(_label);
            await _applicationDatabaseContext.SaveChangesAsync();

            // Get all the labelList
            var response = await _client.GetAsync("/api/labels?sort=id,desc");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var json = JToken.Parse(await response.Content.ReadAsStringAsync());
            json.SelectTokens("$.[*].id").Should().Contain(_label.Id);
            json.SelectTokens("$.[*].label").Should().Contain(DefaultName);
        }

        [Fact]
        public async Task GetLabel()
        {
            // Initialize the database
            _applicationDatabaseContext.Labels.Add(_label);
            await _applicationDatabaseContext.SaveChangesAsync();

            // Get the label
            var response = await _client.GetAsync($"/api/labels/{_label.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var json = JToken.Parse(await response.Content.ReadAsStringAsync());
            json.SelectTokens("$.id").Should().Contain(_label.Id);
            json.SelectTokens("$.label").Should().Contain(DefaultName);
        }

        [Fact]
        public async Task GetNonExistingLabel()
        {
            var response = await _client.GetAsync("/api/labels/" + long.MaxValue);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task UpdateLabel()
        {
            // Initialize the database
            _applicationDatabaseContext.Labels.Add(_label);
            await _applicationDatabaseContext.SaveChangesAsync();

            var databaseSizeBeforeUpdate = _applicationDatabaseContext.Labels.Count();

            // Update the label
            var updatedLabel = await _applicationDatabaseContext.Labels.SingleOrDefaultAsync(it => it.Id == _label.Id);
            // Disconnect from session so that the updates on updatedLabel are not directly saved in db
//TODO detach
            updatedLabel.Name = UpdatedName;

            var response = await _client.PutAsync("/api/labels", TestUtil.ToJsonContent(updatedLabel));
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Validate the Label in the database
            var labelList = _applicationDatabaseContext.Labels.ToList();
            labelList.Count().Should().Be(databaseSizeBeforeUpdate);
            var testLabel = labelList[labelList.Count - 1];
            testLabel.Name.Should().Be(UpdatedName);
        }

        [Fact]
        public async Task UpdateNonExistingLabel()
        {
            var databaseSizeBeforeUpdate = _applicationDatabaseContext.Labels.Count();

            // If the entity doesn't have an ID, it will throw BadRequestAlertException
            var response = await _client.PutAsync("/api/labels", TestUtil.ToJsonContent(_label));
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            // Validate the Label in the database
            var labelList = _applicationDatabaseContext.Labels.ToList();
            labelList.Count().Should().Be(databaseSizeBeforeUpdate);
        }

        [Fact]
        public async Task UpdateLabelWithManyToManyAssociation()
        {
            // Due to JsonIgnore annotation on Operations property, we first create the Label
            // This allow the operation to create the relationship
            _applicationDatabaseContext.Labels.Add(_label);
            await _applicationDatabaseContext.SaveChangesAsync();
            var updatedLabel = await _applicationDatabaseContext.Labels
                .SingleOrDefaultAsync(it => it.Id == _label.Id);

            // Create an Operation to test the ManyToMany association
            var operation = new Operation {
                Date = DateTime.UnixEpoch,
                Description = "AAAAAAAAAA",
                Amount = new decimal(1.0)
            };
            operation.Labels.Add(updatedLabel);
            _applicationDatabaseContext.Operations.Add(operation);
            await _applicationDatabaseContext.SaveChangesAsync();
            operation = await _applicationDatabaseContext.Operations
                .SingleOrDefaultAsync(o => o.Id == operation.Id);

            var databaseSizeBeforeUpdate = _applicationDatabaseContext.Labels.Count();

            // Update the label
            updatedLabel = await _applicationDatabaseContext.Labels
                .SingleOrDefaultAsync(it => it.Id == _label.Id);
            // Disconnect from session so that the updates on updatedLabel are not directly saved in db
            //TODO detach
            updatedLabel.Name = UpdatedName;

            var response = await _client.PutAsync("/api/labels", TestUtil.ToJsonContent(updatedLabel));
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            updatedLabel = await _applicationDatabaseContext.Labels
                .SingleOrDefaultAsync(it => it.Id == _label.Id);

            // Create a second Operation to update the ManyToMany association
            var secondOperation = new Operation {
                Date = DateTime.Now,
                Description = "BBBBBBBBBB",
                Amount = new decimal(2.0)
            };
            secondOperation.Labels.Add(updatedLabel);
            _applicationDatabaseContext.Operations.Add(secondOperation);
            await _applicationDatabaseContext.SaveChangesAsync();
            secondOperation = await _applicationDatabaseContext.Operations
                .SingleOrDefaultAsync(o => o.Id == secondOperation.Id);

            // Validate the Label in the database
            var labelList = _applicationDatabaseContext.Labels
                .Include("OperationLabels.Operation")
                .ToList();
            labelList.Count().Should().Be(databaseSizeBeforeUpdate);
            updatedLabel = labelList[labelList.Count - 1];
            updatedLabel.Name.Should().Be(UpdatedName);
            updatedLabel.Operations[0].Should().Be(operation);
            updatedLabel.Operations[1].Should().Be(secondOperation);                       
        }
    }
}
