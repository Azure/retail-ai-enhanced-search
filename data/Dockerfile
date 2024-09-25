# Use an official Python runtime as the base image
FROM python:3.12.5-slim-bookworm

# Set the working directory in the container
WORKDIR /app

# Copy the requirements file into the container
COPY requirements.txt .

# Install the Python dependencies
RUN pip3 install --no-cache-dir -r requirements.txt

# Copy the rest of the application code into the container, except .env file
COPY . .

RUN chmod +x ./AzureSearch/createIndex.py

# # Set the command to run the application
ENTRYPOINT python ./AzureSearch/createIndex.py
