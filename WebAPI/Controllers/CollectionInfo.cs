using Microsoft.AspNetCore.Mvc;
using ExerciseEngine;
using ExerciseEngine.API;

namespace WebAPI.Controllers
{
    [ApiController]
	[Route("[controller]")]
	public class CollectionInfo : ControllerBase {
		private static readonly string[] Summaries = new[]
		{
			"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
		};

		private readonly ILogger<CollectionInfo> _logger;

		public CollectionInfo(ILogger<CollectionInfo> logger) {
			_logger = logger;
		}

		[HttpGet(Name = "CollectionInfo")]
		public IEnumerable<Exercise_MetaData> Get() {
			return MetadataBuilder();
		}

		private List<Exercise_MetaData> MetadataBuilder() {
			Exercise_MetaData prvniPriklad = new() {
				uniqueId = 1,
				localizations = new() { Language.en, Language.pl, Language.cs, Language.ua },
				variantsCount = 121,
				exerciseType = ExerciseType.Numerical,
				titles = new() {
					{ Language.en, "Multiplication up to 100"},
					{ Language.cs, "Násobení do 100"},
					{ Language.pl, "Polské násobení do 100"},
					{ Language.ua, "Ukrajinské násobení navíc v azbuce 100"}
				},
				descriptions = new() {
					{ Language.en, "121 multiplication operations with both factors ranging from 0 to 10. Since multiplication with 1 and 0 is trivial, and the probability of one of those factors occuring in the problem is 40/121 => each third time, which is too often for the trivial case, the probability of either zero or one occcuring in either factor has been decreased to 5% -> each twentieth exercise. See info image to better understand how we modified the uniform event space."},
					{ Language.cs, "Hezky cesky popisek"},
					{ Language.pl, "Hezky polsky popisek"},
					{ Language.ua, "Hezky UA popisek navíc v azbuce"}
				},
				topics = new() {
					Topic.Multiplication
				},
				grades = new() {
					Grade.Second, Grade.Third
				},
				thumbnailPath = "cestaKSouboru.jpg",
						metaVersion = 1
			};

			Exercise_MetaData druhyPriklad = new() {
				uniqueId = 2,
				localizations = new() { Language.en, Language.pl, Language.cs, Language.ua },
				variantsCount = 8500,
				exerciseType = ExerciseType.Numerical,
				titles = new() {
				{ Language.en, "Arithmetic up to 100 - without two digit multiplication and dividing by two digit numbers."},
				{ Language.cs, "Aritmetika do 100"},
				{ Language.pl, "Polská aritmetika do 100"},
				{ Language.ua, "Ukrajinská aritmetika navíc v azbuce 100"}
			},
					descriptions = new() {
				{ Language.en, "Mix of addition, subtraction, multiplication and division. Multiplication is limited only to both factors being ten or smaller. Similarily division has limited divisor to be at most ten."},
				{ Language.cs, "Hezky cesky popisek o aritemtice"},
				{ Language.pl, "Hezky polsky popisek o aritemtice"},
				{ Language.ua, "Hezky UA popisek navíc v azbuce o aritemtice"}
			},
					topics = new() {
				Topic.Addition, Topic.Subtraction, Topic.Multiplication, Topic.Division, Topic.Arithmetic
			},
					grades = new() {
				Grade.Second, Grade.Third, Grade.Fourth
			},
					thumbnailPath = "jinaCestaKSouboru.jpg",
					metaVersion = 1
			};

			List<Exercise_MetaData> exerciseCollection = new();
			exerciseCollection.Add(prvniPriklad);
			exerciseCollection.Add(druhyPriklad);
			return exerciseCollection;
		}
	}
}