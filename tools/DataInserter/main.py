import json
import uuid
import psycopg2
from datetime import datetime
import os

# Database connection
conn = psycopg2.connect(
    dbname="journeyapp",
    user="postgres",
    password="postgres",
    host="localhost",
    port="5432"
)
cursor = conn.cursor()

# Function to insert JSON data into the database
def insert_data(json_file_path):
    with open(json_file_path, 'r', encoding='utf-8') as file:
        trips_data = json.load(file)

    for trip in trips_data:
        trip_id = str(uuid.uuid4())
        cursor.execute("""
            INSERT INTO public."Trips" ("Id", "Price", "StartDate", "EndDate", "Country", "City", "Description")
            VALUES (%s, %s, %s, %s, %s, %s, %s)
            """, (
                trip_id,
                trip["price"],
                datetime.strptime(trip["start_date"], "%Y-%m-%d").date(),
                datetime.strptime(trip["end_date"], "%Y-%m-%d").date(),
                trip["country"],
                trip["city"],
                trip["description"]
            )
        )

        for day in trip["days"]:
            day_id = str(uuid.uuid4())
            cursor.execute("""
                INSERT INTO public."TripDays" ("Id", "TripId", "Day", "City")
                VALUES (%s, %s, %s, %s)
                """, (
                    day_id,
                    trip_id,
                    day["day"],
                    day["city"]
                )
            )

            for place in day["places"]:
                place_id = str(uuid.uuid4())
                cursor.execute("""
                    INSERT INTO public."TripPlaces" ("Id", "TripDayId", "Title", "Description", "Price")
                    VALUES (%s, %s, %s, %s, %s)
                    """, (
                        place_id,
                        day_id,
                        place["title"],
                        place["description"],
                        place["price"]
                    )
                )

    # Commit changes to the database
    conn.commit()
    print(f"Data insertion completed for {json_file_path}.")

# Function to find all JSON files in a directory
def process_json_files(directory):
    # Iterate over all files in the directory
    for filename in os.listdir(directory):
        # Check if the file has a .json extension
        if filename.endswith(".json"):
            # Construct the full file path
            json_file_path = os.path.join(directory, filename)
            # Call the insert_data function for each JSON file
            insert_data(json_file_path)

# Path to the directory containing JSON files
json_directory = 'data'

# Process all JSON files in the directory
process_json_files(json_directory)

# Close the connection
cursor.close()
conn.close()