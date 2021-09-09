# Packing Slip Rule Engine

This project is an API solution which gives you packing slip as json in return based on different product purchase

# Assumptions

- Due to avoid unnessesary complexity there is no actual DB transaction
- Products are coming from JSON File
- Actual Email Sending code skipped
-

# Important Links

Test Url: https://ruleengine.azurewebsites.net/swagger/index.html
Git Repo URL: https://github.com/chakrabortyanirban/rule-engine
Image Url: docker pull anirban1986/ruleengine:{Build_Version_From_Github_Pipeline}

# Technical Details

- Freamwork: .net 5,0
- Test Freamwork: Nunit
- Documentation: Swagger
- Image Storage: hub.docker.com
- Solution Hosted: Azure app service via docker container (Free tire)
