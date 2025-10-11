# Test Profile Details Fix
Write-Host "Testing Profile Details Fix..." -ForegroundColor Green

# Generate unique timestamp
$timestamp = Get-Date -Format "yyyyMMddHHmmss"

# Step 1: Register a user with profile details
Write-Host "Step 1: Registering user with profile details..." -ForegroundColor Yellow
try {
    $registerBody = @{
        email = "test$timestamp@email.com"
        password = "test1234"
        firstName = "John"
        lastName = "Doe"
        phone = "+1234567890"
        address = "123 Main Street, City, State"
    } | ConvertTo-Json

    $registerResponse = Invoke-RestMethod -Uri "http://localhost:5000/api/Auth/register" -Method POST -ContentType "application/json" -Body $registerBody
    Write-Host "Registration successful!" -ForegroundColor Green
    Write-Host "Registration Response Profile:" -ForegroundColor Cyan
    Write-Host "  FullName: $($registerResponse.profile.fullName)" -ForegroundColor White
    Write-Host "  Address: $($registerResponse.profile.address)" -ForegroundColor White
    Write-Host "  Phone: $($registerResponse.profile.phone)" -ForegroundColor White
    
    $token = $registerResponse.token
} catch {
    Write-Host "Registration failed: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

# Step 2: Login to get fresh token
Write-Host "Step 2: Logging in..." -ForegroundColor Yellow
try {
    $loginBody = @{
        email = "test$timestamp@email.com"
        password = "test1234"
    } | ConvertTo-Json

    $loginResponse = Invoke-RestMethod -Uri "http://localhost:5000/api/Auth/login" -Method POST -ContentType "application/json" -Body $loginBody
    Write-Host "Login successful!" -ForegroundColor Green
    $token = $loginResponse.token
} catch {
    Write-Host "Login failed: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

# Step 3: Access Profile with token
Write-Host "Step 3: Accessing Profile with token..." -ForegroundColor Yellow
try {
    $headers = @{
        "Authorization" = "Bearer $token"
    }
    
    $profileResponse = Invoke-RestMethod -Uri "http://localhost:5000/api/Profile" -Method GET -Headers $headers
    Write-Host "Profile access successful!" -ForegroundColor Green
    Write-Host "Profile Details:" -ForegroundColor Cyan
    Write-Host "  ID: $($profileResponse.id)" -ForegroundColor White
    Write-Host "  UserId: $($profileResponse.userId)" -ForegroundColor White
    Write-Host "  FullName: $($profileResponse.fullName)" -ForegroundColor White
    Write-Host "  Address: $($profileResponse.address)" -ForegroundColor White
    Write-Host "  Phone: $($profileResponse.phone)" -ForegroundColor White
    Write-Host "  Bio: $($profileResponse.bio)" -ForegroundColor White
    Write-Host "  BirthDate: $($profileResponse.birthDate)" -ForegroundColor White
} catch {
    Write-Host "Profile access failed: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "Profile details test completed!" -ForegroundColor Green
