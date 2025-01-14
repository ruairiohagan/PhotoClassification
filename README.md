# PhotoClassification

A widnows application that uses LM Studio API to classify and describe image files then allows keyword searches to be performed to quickly find images.

## Installation

### Overview
The app uses [LM Studio](https://lmstudio.ai/) to classify the images, so this must be installed and configured first before any classification can be done (however, once classification is complete, searches do not need a conneciton to LM Studio).

In my testing I used the **llava-1.6-mistral-7b** model which produced impressive results for me while taking 10 to 15 seconds to index an image 
on a system with 64Gb RAM and a 16Gb graphics card, so you may want to experiment with other models for better performance.


### Step by Step

1. Download and install [LM Studio](https://lmstudio.ai/).
1. Open LM Studio and select the Discover Icon on the left of the screen
1. Search for the "llava-1.6-mistral-7b" model (or you can use any other model with the "Vision Enabled" tag) and download it
1. Click the "Developer" icon on the left and click "Select a model to load", then select the model you downloaded above
1. Make sure the "CPU Offload" is set as high as your GPU can support (0 of you have none!)
1. Click "Remember Settings" and then "Load Model"
1. Make sure status is set to "Running" in the top left