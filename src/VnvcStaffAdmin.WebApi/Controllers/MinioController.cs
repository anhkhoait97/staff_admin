using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using VnvcStaffAdmin.Application.ExtendServices.Minio;
using VnvcStaffAdmin.Domain.Model;
using VnvcStaffAdmin.Domain.ResponseModel;

namespace VnvcStaffAdmin.WebApi.Controllers
{
    public class MinioController : AuthControllerBase
    {
        private readonly IMinioService _minioService;
        private readonly ILogger<MinioController> _logger;

        public MinioController(IMinioService minioService, ILogger<MinioController> logger)
        {
            _minioService = minioService;
            _logger = logger;
        }

        [HttpPost("upload")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FileUploadResponse>> UploadFile(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("No file uploaded");

                // Validate file size (e.g., 10MB max)
                if (file.Length > 10 * 1024 * 1024)
                    return BadRequest("File size exceeds maximum limit of 10MB");

                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                using var stream = file.OpenReadStream();

                string url = await _minioService.UploadFileAsync(fileName, stream, file.ContentType);

                var result = new FileUploadResponse
                {
                    FileName = fileName,
                    OriginalFileName = file.FileName,
                    FileUrl = url,
                    ContentType = file.ContentType,
                    FileSize = file.Length
                };

                return Ok(ResponseModel.Successed("File uploaded successfully", result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file");
                return StatusCode(500, "An error occurred while uploading the file");
            }
        }

        [HttpGet("download/{fileName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DownloadFile([Required] string fileName)
        {
            try
            {
                var fileStream = await _minioService.DownloadFileAsync(fileName);

                // Get file metadata to determine content type
                var metadata = await _minioService.GetFileMetadataAsync(fileName) as dynamic;
                string contentType = metadata?.ContentType ?? "application/octet-stream";

                return File(fileStream, contentType, fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error downloading file: {FileName}", fileName);
                return NotFound($"File {fileName} not found or could not be accessed");
            }
        }

        [HttpDelete("{fileName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteFile([Required] string fileName)
        {
            try
            {
                bool result = await _minioService.DeleteFileAsync(fileName);

                if (result)
                    return Ok(ResponseModel.Successed($"File {fileName} deleted successfully"));

                return NotFound($"File {fileName} not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting file: {FileName}", fileName);
                return StatusCode(500, "An error occurred while deleting the file");
            }
        }

        [HttpGet("list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<string>>> ListFiles()
        {
            try
            {
                var files = await _minioService.ListFilesAsync();
                return Ok(ResponseModel.Successed("Get list files successfully", files));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listing files");
                return StatusCode(500, "An error occurred while listing files");
            }
        }

        [HttpGet("metadata/{fileName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFileMetadata([Required] string fileName)
        {
            try
            {
                var metadata = await _minioService.GetFileMetadataAsync(fileName);
                return Ok(ResponseModel.Successed("Get file metadata successfully", metadata));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting metadata for file: {FileName}", fileName);
                return NotFound($"File {fileName} not found or could not be accessed");
            }
        }

        /// <summary>
        /// Generates a pre-signed URL for a file
        /// </summary>
        /// <param name="fileName">The name of the file</param>
        /// <returns>A pre-signed URL to access the file</returns>
        [HttpGet("presigned-url/{fileName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> GetPreSignedUrl([Required] string fileName)
        {
            try
            {
                string url = await _minioService.GeneratePreSignedURLAsync(fileName);
                return Ok(ResponseModel.Successed("Generate Presigned Url successfully", url));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating pre-signed URL for file: {FileName}", fileName);
                return NotFound($"File {fileName} not found or could not be accessed");
            }
        }
    }
}
