from bs4 import BeautifulSoup
import requests
import json
from azure.storage import CloudStorageAccount

from azure.storage.table import TableService, TableBatch
from slugify import slugify


class Exercise:
    name = ""
    muscleTargeted = ""
    url = ""
    #images = []

    def __init__(self, name, muscleTargeted, url):
        self.name = name
        self.muscleTargeted = muscleTargeted
        self.url = url

baseUrl = "https://www.bodybuilding.com"
url = baseUrl + "/exercises/finder"

page = 1

workToDo = True
exercises = []

batch = TableBatch()

account = CloudStorageAccount(is_emulated=True)
table_service = account.create_table_service()
table_service.create_table('exercises')

counter = 0

while(workToDo):
    headers = { "x-bb-ex-skip-finder-form": "YES" }

    urlToGet = url + "/" + str(page)
    print(urlToGet)
    r = requests.get(urlToGet, headers=headers)

    soup = BeautifulSoup(r.text, "html.parser")

    exerciseDivs = soup.findAll("div", { "class": "ExResult-row"})

    currentCounter = len(exercises)
    for exerciseDiv in exerciseDivs:
        name = exerciseDiv.find("h3").find("a").text.strip()
        exerciseUrl = baseUrl + exerciseDiv.find("h3").find("a")['href']
        muscleTargeted = exerciseDiv.find("div", { "class": "ExResult-muscleTargeted"}).find("a").text.strip()
        exercise = Exercise(name, muscleTargeted, exerciseUrl) 
        images = []
        for img in exerciseDiv.findAll("img"):
          images.append(img["data-src"])
          # print("Name: " + exercise.name + ". Targets: " + exercise.muscleTargeted + " URL: " + exercise.url)
        # exercises.append(exercise)
        equipmentTypeDiv = exerciseDiv.find("div", {"class": "ExResult-equipmentType"})
        exercise = { 
            "PartitionKey": "exercises", 
            "RowKey": slugify(name), 
            "Name": name, 
            "Url": 
            exerciseUrl, 
            "MuscleTargeted": muscleTargeted, 
            "Image_1": (images and images[0] or ""), "Image_2": (len(images) > 1 and images[1] or ""), 
            "EquipmentType": equipmentTypeDiv.find("a").text.strip(), 
            "EquipmentTypeUrl": equipmentTypeDiv.find("a")["href"]}
        exercises.append(exercise)
        table_service.insert_or_replace_entity('exercises', exercise)
        #batch.insert_entity(exercise)
        #counter += 1
        #if counter == 100:
        #    print('Commiting batch.')
        #    table_service.commit_batch('exercises', batch)            
        #    batch = TableBatch()
        #    counter = 0

    print("Got " + str(len(exercises) - currentCounter) + " from " + urlToGet)
    workToDo = len(exercises) - currentCounter  > 0
    page += 1

f = open('exercises.json', "w")
print(json.dump(exercises, f))

print('Commiting batch.')
table_service.commit_batch('exercises', batch)
