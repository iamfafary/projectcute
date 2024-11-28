using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Supabase;
using Supabase.Storage;
using System.IO;


namespace Gradpath
{
    public class SupabaseClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _supabaseUrl;
        private readonly string _supabaseKey;

        public SupabaseClient(string supabaseUrl, string supabaseKey)
        {
            supabaseKey = supabaseKey.Trim(); 

            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(supabaseUrl);
            _httpClient.DefaultRequestHeaders.Add("apikey", supabaseKey);
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {supabaseKey}");
            _supabaseUrl = supabaseUrl;
            _supabaseKey = supabaseKey;
        }

        public async Task<string> RegisterUser(string email, string password)
        {
            try
            {
                var signUpResponse = await _httpClient.PostAsync(
                    $"{_supabaseUrl}/auth/v1/signup",
                    new StringContent(
                        JsonSerializer.Serialize(new { email, password }),
                        Encoding.UTF8,
                        "application/json"
                    )
                );

                var signUpResponseBody = await signUpResponse.Content.ReadAsStringAsync();
                Console.WriteLine($"Sign Up Response: {signUpResponseBody}");

                if (!signUpResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Registration failed: {signUpResponse.ReasonPhrase}");
                    return null;
                }

                using (JsonDocument doc = JsonDocument.Parse(signUpResponseBody))
                {
                    JsonElement root = doc.RootElement;
                    if (root.TryGetProperty("user", out JsonElement userElement))
                    {
                        string userId = userElement.GetProperty("id").GetString();
                        Console.WriteLine($"Extracted user ID: {userId}");
                        return userId;
                    }
                    else
                    {
                        Console.WriteLine("User ID not found in sign up response.");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during registration: {ex.Message}");
                return null;
            }
        }

        public async Task<Student> AuthenticateUser(string email, string password)
        {
            try
            {
                var authResponse = await _httpClient.PostAsync(
                    $"{_supabaseUrl}/auth/v1/token?grant_type=password",
                    new StringContent(
                        JsonSerializer.Serialize(new { email, password }),
                        Encoding.UTF8,
                        "application/json"
                    )
                );

                var authResponseBody = await authResponse.Content.ReadAsStringAsync();
                Console.WriteLine($"Auth Response: {authResponseBody}");

                if (authResponse.IsSuccessStatusCode)
                {
                    using (JsonDocument doc = JsonDocument.Parse(authResponseBody))
                    {
                        JsonElement root = doc.RootElement;
                        if (root.TryGetProperty("user", out JsonElement userElement))
                        {
                            if (userElement.TryGetProperty("id", out JsonElement userIdElement))
                            {
                                string userId = userIdElement.GetString();

                                // Fetch student details
                                var student = await GetStudentDetails(Guid.Parse(userId));
                                if (student != null)
                                {
                                    return student;
                                }
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during authentication: {ex.Message}");
                return null;
            }
        }

        public async Task<Counselor> AuthenticateCounselor(string email, string password)
        {
            try
            {
                var authResponse = await _httpClient.PostAsync(
                    $"{_supabaseUrl}/auth/v1/token?grant_type=password",
                    new StringContent(
                        JsonSerializer.Serialize(new { email, password }),
                        Encoding.UTF8,
                        "application/json"
                    )
                );

                var authResponseBody = await authResponse.Content.ReadAsStringAsync();
                Console.WriteLine($"Auth Response: {authResponseBody}");

                if (authResponse.IsSuccessStatusCode)
                {
                    using (JsonDocument doc = JsonDocument.Parse(authResponseBody))
                    {
                        JsonElement root = doc.RootElement;
                        if (root.TryGetProperty("user", out JsonElement userElement))
                        {
                            if (userElement.TryGetProperty("id", out JsonElement userIdElement))
                            {
                                string userId = userIdElement.GetString();

                                // Fetch counselor details
                                var counselor = await GetCounselorDetails(Guid.Parse(userId));
                                if (counselor != null)
                                {
                                    return counselor;
                                }
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during authentication: {ex.Message}");
                return null;
            }
        }

        public async Task<Student> GetStudentDetails(Guid userId)
        {
            var response = await _httpClient.GetAsync($"/rest/v1/students?id=eq.{userId}");
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Get Student Details Response: {responseBody}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to retrieve student details: {response.ReasonPhrase}");
                return null;
            }

            var studentDetails = JsonSerializer.Deserialize<List<Student>>(responseBody)?.FirstOrDefault();
            return studentDetails;
        }

        public async Task<Counselor> GetCounselorDetails(Guid userId)
        {
            var response = await _httpClient.GetAsync($"/rest/v1/counselors?id=eq.{userId}");
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Get Counselor Details Response: {responseBody}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to retrieve counselor details: {response.ReasonPhrase}");
                return null;
            }

            var counselorDetails = JsonSerializer.Deserialize<List<Counselor>>(responseBody)?.FirstOrDefault();
            return counselorDetails;
        }


        public string BaseUrl => _supabaseUrl;

        public async Task<bool> InsertCounselorDetails(Guid userId, string fullName, string schoolName, int yearsOfExperience, string profilePictureUrl)
        {
            var counselorDetails = new
            {
                id = userId,
                full_name = fullName,
                school_name = schoolName,
                years_of_experience = yearsOfExperience,
                profile_picture_url = profilePictureUrl
            };

            var jsonString = JsonSerializer.Serialize(counselorDetails);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/rest/v1/counselors", content);
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Insert Counselor Response: {responseBody}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to insert counselor details: {response.ReasonPhrase}");
                return false;
            }

            Console.WriteLine($"Counselor details inserted successfully for user ID: {userId}");
            return true;
        }


        public async Task<bool> InsertStudentDetails(Guid userId, string fullName, string careerInterests, string skills, string goals, string profilePictureUrl)
        {
            var studentDetails = new
            {
                id = userId,
                full_name = fullName,
                career_interests = careerInterests,
                skills = skills,
                goals = goals,
                profile_picture_url = profilePictureUrl
            };

            var jsonString = JsonSerializer.Serialize(studentDetails);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/rest/v1/students", content);
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Insert Student Response: {responseBody}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to insert student details: {response.ReasonPhrase}");
                return false;
            }

            Console.WriteLine($"Student details inserted successfully for user ID: {userId}");
            return true;
        }
        public async Task<string> UploadAndSaveProfilePicture(string userId, Stream fileStream, string fileName, string userType)
        {
            try
            {
                // Prepare the multipart form data content
                var content = new MultipartFormDataContent();
                var fileContent = new StreamContent(fileStream);
                fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                {
                    Name = "file",
                    FileName = fileName
                };
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

                content.Add(fileContent);

                // Send the request to upload the file
                var response = await _httpClient.PostAsync($"/storage/v1/object/profile-pictures/{fileName}", content);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Construct the public URL
                    string publicUrl = $"{_supabaseUrl}/storage/v1/object/public/profile-pictures/{fileName}";

                    // Update the user's profile with the profile picture URL
                    bool isUpdated = await UpdateUserProfilePictureUrl(userId, publicUrl, userType);
                    if (isUpdated)
                    {
                        return publicUrl;
                    }
                    else
                    {
                        Console.WriteLine("Failed to update user profile with the profile picture URL.");
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine($"Upload failed with status code: {response.StatusCode}, response: {responseBody}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during upload: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> UpdateUserProfilePictureUrl(string userId, string profilePictureUrl, string userType)
        {
            var updateData = new
            {
                profile_picture_url = profilePictureUrl
            };

            var jsonString = JsonSerializer.Serialize(updateData);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            // Determine the table based on user type (student or counselor)
            string tableName = userType == "student" ? "students" : "counselors";

            var response = await _httpClient.PatchAsync($"/rest/v1/{tableName}?id=eq.{userId}", content);
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Update User Profile Picture URL Response: {responseBody}");

            return response.IsSuccessStatusCode;
        }



        // Check if the user already set the details
        public async Task<bool> CheckUserDetails(Guid userId)
        {
            try
            {
                // Check students table
                var response = await _httpClient.GetAsync($"/rest/v1/students?id=eq.{userId}");
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Check Student Details Response: {responseBody}");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Failed to retrieve student details: {response.ReasonPhrase}");
                }
                else
                {
                    var studentDetails = JsonSerializer.Deserialize<List<Student>>(responseBody)?.FirstOrDefault();
                    if (studentDetails != null && !string.IsNullOrEmpty(studentDetails.FullName))
                    {
                        Console.WriteLine($"Student details found: {studentDetails.FullName}");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Student details not found or incomplete.");
                    }
                }

                // Check counselors table if student details are not found
                response = await _httpClient.GetAsync($"/rest/v1/counselors?id=eq.{userId}");
                responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Check Counselor Details Response: {responseBody}");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Failed to retrieve counselor details: {response.ReasonPhrase}");
                }
                else
                {
                    var counselorDetails = JsonSerializer.Deserialize<List<Counselor>>(responseBody)?.FirstOrDefault();
                    if (counselorDetails != null && !string.IsNullOrEmpty(counselorDetails.FullName))
                    {
                        Console.WriteLine($"Counselor details found: {counselorDetails.FullName}");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Counselor details not found or incomplete.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception checking user details: {ex.Message}");
            }

            return false;
        }


        public async Task<Guid> LoginUser(string email, string password)
        {
            var loginPayload = new
            {
                email = email,
                password = password
            };

            var jsonString = JsonSerializer.Serialize(loginPayload);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/auth/v1/token?grant_type=password", content);
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Login Response: {responseBody}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Login failed: {response.ReasonPhrase}");
            }

            using (JsonDocument doc = JsonDocument.Parse(responseBody))
            {
                JsonElement root = doc.RootElement;
                if (root.TryGetProperty("user", out JsonElement userElement) &&
                    userElement.TryGetProperty("id", out JsonElement userIdElement))
                {
                    string userId = userIdElement.GetString();
                    return Guid.Parse(userId);
                }
                else
                {
                    throw new Exception("User ID not found in login response.");
                }
            }
        }

        // Check existing email
        public async Task<bool> EmailExists(string email)
        {
            var response = await _httpClient.GetAsync($"/rest/v1/users?email=eq.{email}");
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Check Email Exists Response: {responseBody}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to check email: {response.ReasonPhrase}");
            }

            var existingUser = JsonSerializer.Deserialize<List<Student>>(responseBody)?.FirstOrDefault();
            return existingUser != null;
        }

        // Methods to get and save user roles
        public async Task<string> GetUserRole(Guid userId)
        {
            var response = await _httpClient.GetAsync($"/rest/v1/roles?user_id=eq.{userId}");
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Get User Role Response: {responseBody}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to retrieve user role: {response.ReasonPhrase}");
                return null;
            }

            var userRole = JsonSerializer.Deserialize<List<UserRole>>(responseBody)?.FirstOrDefault();
            return userRole?.role;
        }

        public async Task<bool> SaveUserRole(Guid userId, string role)
        {
            var roleData = new
            {
                user_id = userId,
                role = role
            };

            var jsonString = JsonSerializer.Serialize(roleData);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            Console.WriteLine($"Requesting to save user role: {jsonString}");

            try
            {
                var response = await _httpClient.PostAsync("/rest/v1/roles", content);
                var responseBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"Save User Role Response: {responseBody}");
                Console.WriteLine($"Status Code: {response.StatusCode}");
                Console.WriteLine($"Reason Phrase: {response.ReasonPhrase}");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Failed to save user role: {response.ReasonPhrase}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during saving user role: {ex.Message}");
                return false;
            }
        }

        //for counselor dashboard
        public async Task<List<Student>> GetStudents()
        {
            var response = await _httpClient.GetAsync("/rest/v1/students");
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Student>>(responseBody);
        }
        //feedback
        public async Task<bool> AddFeedback(string studentId, string counselorId, string comment)
        {
            // Ensure the user is a counselor
            var isCounselor = await IsUserCounselor(counselorId);
            if (!isCounselor)
            {
                return false; // Only counselors can add feedback
            }

            var feedback = new
            {
                student_id = studentId,
                counselor_id = counselorId,
                comment = comment,
                created_at = DateTime.UtcNow
            };

            var jsonString = JsonSerializer.Serialize(feedback);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/rest/v1/feedback", content);
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Add Feedback Response: {responseBody}");

            return response.IsSuccessStatusCode;
        }

        private async Task<bool> IsUserCounselor(string userId)
        {
            var response = await _httpClient.GetAsync($"/rest/v1/counselors?id=eq.{userId}");
            var responseBody = await response.Content.ReadAsStringAsync();
            var counselors = JsonSerializer.Deserialize<List<Counselor>>(responseBody);
            return counselors?.Any() == true;
        }

        /*public async Task<List<Feedback>> GetFeedbackForStudent(string studentId)
        {
            var response = await _httpClient.GetAsync($"/rest/v1/feedback?student_id=eq.{studentId}");
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Feedback>>(responseBody);
        }*/
        public async Task<List<Feedback>> GetFeedbackForStudent(string studentId)
        {
            var response = await _httpClient.GetAsync($"/rest/v1/feedback?student_id=eq.{studentId}&select=comment,counselor:counselor_id(full_name)");
            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"API Response: {responseBody}");  // Debug statement
            return JsonSerializer.Deserialize<List<Feedback>>(responseBody);
        }
    }
}