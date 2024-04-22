To Create a StoryBoard

1: Create empty GameObject and attach StoryHandler.cs and create yor own design under this object
2: And then make a prefab of this
3: mark this prefab as Addressables.
4. Confirm it has been added to the Addressables group in Windows>AssetManagement>Addressables>Group
5: Then Attach this storyboard prefab to StoryBoardRefInOrder.asset a scriptable object (Assets\prefab\ScriptableObjects)
6: Note: The Order In which the StoryBoard attached will be the order of View
7: Done


To Make a Manipulation Object

1: Drag SelectorPrefab() inside any gameobject/model
2: Attach the parent gameObject which is to be manipulated.
3: Adjust the Box Collider as per size needed
4: Done


To Make/Update Addressables
1: Go To Windows Tab -> Asset management -> Click On 'Addressables' 
2: To Create a New Buid: -> Click on Build dropDown tab in Top Middle ->  'New Build' -> Default Build Settings
3: To Update the previous Build -> Click on Build dropDown tab in Top Middle -> click on 'update previous Build' -> Select the file as per plateform specific folder file Like AddressableAssetsData\Android\addressables_content_state.bin
4: Note: Maybe a Issue Can arise becoz of path not found
	Solution: Go To Addressables Group -> Tools ->Click Inspect System Settings-> Then Click on Manage settings and 		Change Remote Build Path
5: Done


To Push the Addressables resources to Cloud
1: Login to Blob Storage
2: Go to https://spheregenmixedreality.blob.core.windows.net/spheregenxrguide/[BuildTarget]
3: and paste the 4 files which are updated/created from local Remote Path.
4: Done.

To test locally you can set Windows>Manage>Addressables>Groups and set Use Asset Database for local loading.
To test actually pulling form the server set Windows>Manage>Addressables>Groups and set Use Existing Build to have the application pull from blob storage.

To Build Asset Bundle from Instant Google
1:  Go to Google Tab -> Play Instant -> Quick Deplay -> Bundle Creation Tab -> Add your Scene->Click on BuildAssetBundle 	Button -> Choose the Local path
2: Then Go To https://spheregenmixedreality.blob.core.windows.net/spheregenxrguide/AssetBundles/
3: Paste the Asset Bundle





 


 

