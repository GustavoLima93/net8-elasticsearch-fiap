using crop_api_elastic_demo.Services;
using crop_api_elastic_demo.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace crop_api_elastic_demo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CropController : ControllerBase
{
        private readonly ILogger<CropController> _logger;
        private readonly CropService _cropService;

        public CropController(ILogger<CropController> logger, CropService cropService)
        {
                _logger = logger;
                _cropService = cropService;
        }
        
        [HttpGet]
        public ActionResult<List<CropViewModel>> Get() => _cropService.Get();

        [HttpGet("{id:length(24)}", Name = "GetCrop")]
        public ActionResult<CropViewModel> Get(string id)
        {
                var crop = _cropService.Get(id);

                if (crop.Equals(null))
                {
                        return NotFound();
                }
                

                return crop;
        }

        [HttpPost]
        public ActionResult<CropViewModel> Create(CropViewModel crop)
        {
                var result = _cropService.Create(crop);
                return CreatedAtRoute("GetCrop", new { id = result.Id.ToString() }, result);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, CropViewModel cropIn)
        {
                var crop = _cropService.Get(id);
                if (crop.Equals(null))
                        return NotFound();
            
                _cropService.Update(id, cropIn);

                return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
                var crop = _cropService.Get(id);

                if (crop.Equals(null))
                        return NotFound();
            
                _cropService.Remove(crop.Id);

                return NoContent();

        }
        
}
