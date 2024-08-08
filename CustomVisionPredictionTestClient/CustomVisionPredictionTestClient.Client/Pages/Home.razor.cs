using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.Models;

namespace CustomVisionPredictionTestClient.Client.Pages;

public partial class Home
{
	private EditContext _editContext;
	private FormModel _model = new FormModel();
	private ImagePrediction _imagePrediction;

	protected override void OnInitialized()
	{
		_editContext = new EditContext(_model);
	}

	private async Task HandleImageChange(InputFileChangeEventArgs args)
	{
		if (!_editContext.Validate())
		{
			return;
		}

		_imagePrediction = null;

		using var stream = args.File.OpenReadStream();

		using var predictionApi = new CustomVisionPredictionClient(new ApiKeyServiceClientCredentials(_model.PredictionKey))
		{
			Endpoint = _model.PredictionEndpoint
		};

		_imagePrediction = await predictionApi.ClassifyImageAsync(Guid.Parse(_model.ProjectId), _model.IterationId, stream);
	}

	public class FormModel
	{
		[Required]
		public string PredictionKey { get; set; }

		[Required]
		public string PredictionEndpoint { get; set; } = "https://westeurope.api.cognitive.microsoft.com/";

		[Required]
		public string ProjectId { get; set; } = "a829573b-c5a6-402d-b41d-b51d26b62014";

		[Required]
		public string IterationId { get; set; } = "Iteration1";
	}
}
