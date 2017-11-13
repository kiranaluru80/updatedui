using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace SampleDB
{
    public partial class CommunicationsPage : ContentPage
    {
        public List<JSSEMasterCategory> listOfCategarys;
		public List<RatingTable> listOfRatings;
        public List<JSSEMasterBehavior> listOfObjectsPerCategary;
        public CommunicationsPage()
        {
            InitializeComponent();

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

           // listOfObjects = await App.Database.GetJsseBehaviors(0,1);
			await ReadDataFromJson();

            listOfCategarys = await App.Database.GetJsseCategories();
            listOfRatings = await App.Database.GetRatings();
           // listOfObjectsPerCategary = await App.Database.GetJsseBehaviors(0, 1);
            DynamicCategaryDesign(listOfCategarys);

        }


        public async void DynamicCategaryDesign(List<JSSEMasterCategory> listOfCategarys){


            for (int i = 0; i < listOfCategarys.Count; i++)
            {
                StackLayout mainStackLayout = new StackLayout();
                mainStackLayout.Padding = new Thickness(20, 0, 20, 0);
                mainStackLayout.Orientation = StackOrientation.Horizontal;
                mainStackLayout.Spacing = 20;
                mainStackLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
                mainStackLayout.VerticalOptions = LayoutOptions.Start;
                mainStackLayout.BindingContext = listOfCategarys[i];
				
				Label titleLabel = new Label();
                titleLabel.Text = listOfCategarys[i].Category;
                titleLabel.HorizontalOptions = LayoutOptions.FillAndExpand;
                titleLabel.VerticalOptions = LayoutOptions.Center;
                titleLabel.FontSize = 16;
                titleLabel.TextColor = Color.Black;
                titleLabel.FontAttributes = FontAttributes.Bold;

                Image arrowImage = new Image();
                arrowImage.Source = "ArrowDown";
                arrowImage.HorizontalOptions = LayoutOptions.End;
                arrowImage.VerticalOptions = LayoutOptions.Center;
                arrowImage.HeightRequest = 25;
                arrowImage.WidthRequest = 25;

                mainStackLayout.Children.Add(titleLabel);
                mainStackLayout.Children.Add(arrowImage);



                BoxView bottomLine = new BoxView();
                bottomLine.HorizontalOptions = LayoutOptions.FillAndExpand;
                bottomLine.VerticalOptions = LayoutOptions.Start;
                bottomLine.HeightRequest = 1;
                bottomLine.BackgroundColor = Color.Silver;

                CategaryStacklayout.Children.Add(mainStackLayout);
                CategaryStacklayout.Children.Add(bottomLine);
                CategaryStacklayout.StyleId = listOfCategarys[i].Category;

				var communicationTapGestureRecognizer = new TapGestureRecognizer();
                communicationTapGestureRecognizer.Tapped +=  (sender, e) => {

                    StackLayout selectedLayout = sender as StackLayout;
                    StackLayout selecteLayoutParent = selectedLayout.Parent as StackLayout;
                    var selectedObject = selectedLayout.Children.Count;
                    var countVal = selecteLayoutParent.Children.Count;
					// var obj = visibleOrInvisible.StyleId;
					//StackLayout selectedStackChildern = selectedLayout.Parent.FindByName<StackLayout>("isVisibleOrInVisibleRef");

					//listOfObjectsPerCategary = await App.Database.GetJsseBehaviors(0, 1);
                    for (int j = 0; j < selecteLayoutParent.Children.Count; j++){
                        Label selectedLabel = selectedLayout.Children[0] as Label;
                        Image selectedImage = selectedLayout.Children[1] as Image;
                        if (selectedLabel.Text == selecteLayoutParent.Children[j].StyleId){
                            if (selecteLayoutParent.Children[j].IsVisible){
                                selecteLayoutParent.Children[j].IsVisible = false;
                                selectedImage.Source = "ArrowDown";
                            }else{
                                selecteLayoutParent.Children[j].IsVisible = true;
                                selectedImage.Source = "Arrowup";
                            }
                        }
                    }
                    JSSEMasterCategory sec = selectedLayout.BindingContext as JSSEMasterCategory;

				};
				mainStackLayout.GestureRecognizers.Add(communicationTapGestureRecognizer);
				communicationTapGestureRecognizer.NumberOfTapsRequired = 1;

                listOfObjectsPerCategary = await App.Database.GetJsseBehaviors(0, listOfCategarys[i].Category_ID);

                VisibleOrInVisibleScreenDesign(CategaryStacklayout, listOfCategarys[i].Category);
            }
        }

        public void VisibleOrInVisibleScreenDesign(StackLayout categaryStacklayoutRef, string styleIdRef) {

            StackLayout visibleOrInVisibleMainStack = new StackLayout();
            visibleOrInVisibleMainStack.HorizontalOptions = LayoutOptions.FillAndExpand;
            visibleOrInVisibleMainStack.VerticalOptions = LayoutOptions.Start;
            visibleOrInVisibleMainStack.Orientation = StackOrientation.Vertical;
            visibleOrInVisibleMainStack.Spacing = 0;
            visibleOrInVisibleMainStack.IsVisible = false;
            visibleOrInVisibleMainStack.StyleId = styleIdRef;
            //visibleOrInVisibleMainStack.BackgroundColor = Color.Red;

            StackLayout overAllRatingStack = new StackLayout();
            overAllRatingStack.HorizontalOptions = LayoutOptions.FillAndExpand;
            overAllRatingStack.VerticalOptions = LayoutOptions.Start;


            StackLayout titleAndEditBtnStack = new StackLayout();
            titleAndEditBtnStack.Padding = new Thickness(20, 0, 20, 0);
            titleAndEditBtnStack.Orientation = StackOrientation.Horizontal;
            titleAndEditBtnStack.HorizontalOptions = LayoutOptions.FillAndExpand;
            titleAndEditBtnStack.VerticalOptions = LayoutOptions.Start;
            titleAndEditBtnStack.Spacing = 5;

            Label label = new Label();
            label.HorizontalOptions = LayoutOptions.FillAndExpand;
            label.Text = "Overall rating";
            label.FontSize = 14;

            Button editButton = new Button();
            editButton.Image = "edit";
            editButton.HorizontalOptions = LayoutOptions.End;
            editButton.WidthRequest = 20;
            editButton.HeightRequest = 20;

            titleAndEditBtnStack.Children.Add(label);
            titleAndEditBtnStack.Children.Add(editButton);
            overAllRatingStack.Children.Add(titleAndEditBtnStack);

            StackLayout ratingsLayout = new StackLayout();
            ratingsLayout.Orientation = StackOrientation.Vertical;
            ratingsLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
            ratingsLayout.VerticalOptions = LayoutOptions.Start;
            ratingsLayout.Spacing = 5;

            OverAllRatingsDesign(listOfRatings, ratingsLayout);

            overAllRatingStack.Children.Add(ratingsLayout);

            visibleOrInVisibleMainStack.Children.Add(overAllRatingStack);

            //if (listOfObjectsPerCategary != null )
            //{
                
                secondPartLayoutDesign(listOfObjectsPerCategary, visibleOrInVisibleMainStack);
                categaryStacklayoutRef.Children.Add(visibleOrInVisibleMainStack);
            //}





        }

        public void secondPartLayoutDesign(List<JSSEMasterBehavior> listOfObjectsPerCategary, StackLayout secondStackRef){
            
            for (int i = 0; i < listOfObjectsPerCategary.Count; i++)
            {
                StackLayout secondPartStack = new StackLayout();
                secondPartStack.HorizontalOptions = LayoutOptions.FillAndExpand;
                secondPartStack.VerticalOptions = LayoutOptions.Start;
                if (i == 0)
                {
                    secondPartStack.Padding = new Thickness(0, 30, 0, 0);
                }else{
                    secondPartStack.Padding = new Thickness(0, 10, 0, 0);
                }

                StackLayout titleAndEditBtnStack = new StackLayout();
                titleAndEditBtnStack.Padding = new Thickness(20, 0, 20, 0);
                titleAndEditBtnStack.Orientation = StackOrientation.Horizontal;
                titleAndEditBtnStack.HorizontalOptions = LayoutOptions.FillAndExpand;
                titleAndEditBtnStack.VerticalOptions = LayoutOptions.Start;
                titleAndEditBtnStack.Spacing = 5;

                Label label = new Label();
                label.HorizontalOptions = LayoutOptions.FillAndExpand;
                label.Text = listOfObjectsPerCategary[i].Behavior;
                label.FontSize = 14;

                Button editButton = new Button();
                editButton.Image = "edit";
                editButton.HorizontalOptions = LayoutOptions.End;
                editButton.WidthRequest = 20;
                editButton.HeightRequest = 20;

                titleAndEditBtnStack.Children.Add(label);
                titleAndEditBtnStack.Children.Add(editButton);
                secondPartStack.Children.Add(titleAndEditBtnStack);

                StackLayout ratingsLayout = new StackLayout();
                ratingsLayout.Orientation = StackOrientation.Vertical;
                ratingsLayout.HorizontalOptions = LayoutOptions.FillAndExpand;
                ratingsLayout.VerticalOptions = LayoutOptions.Start;
                ratingsLayout.Spacing = 5;

                OverAllRatingsDesign(listOfRatings, ratingsLayout);

                secondPartStack.Children.Add(ratingsLayout);

                secondStackRef.Children.Add(secondPartStack);
            }
        }


		public void OverAllRatingsDesign(List<RatingTable> listOfRatings, StackLayout layout)
		{

			BoxView topLine = new BoxView();
			topLine.HorizontalOptions = LayoutOptions.FillAndExpand;
			topLine.VerticalOptions = LayoutOptions.Start;
			topLine.HeightRequest = 1;
			topLine.BackgroundColor = Color.Silver;
            layout.Children.Add(topLine);


			StackLayout ratingsLayoutSub = new StackLayout();
            ratingsLayoutSub.Orientation = StackOrientation.Horizontal;
			ratingsLayoutSub.HorizontalOptions = LayoutOptions.FillAndExpand;
			ratingsLayoutSub.VerticalOptions = LayoutOptions.Start;
			ratingsLayoutSub.Spacing = 0;
            ratingsLayoutSub.Padding = new Thickness(0);
			

            for (int i = 0; i < listOfRatings.Count; i++)
			{
				Button exceptionalBtn = new Button();
                exceptionalBtn.Text = listOfRatings[i].Rating;
				exceptionalBtn.HorizontalOptions = LayoutOptions.FillAndExpand;
				exceptionalBtn.BackgroundColor = Color.Transparent;
                exceptionalBtn.StyleId = listOfRatings[i].Rating_ID.ToString();
				ratingsLayoutSub.Children.Add(exceptionalBtn);
				

                if (i != listOfRatings.Count - 1)
				{
					BoxView verticalLine = new BoxView();
					verticalLine.HorizontalOptions = LayoutOptions.Start;
					verticalLine.VerticalOptions = LayoutOptions.FillAndExpand;
					verticalLine.WidthRequest = 1;
					verticalLine.BackgroundColor = Color.Silver;
					ratingsLayoutSub.Children.Add(verticalLine);
				}
			}

            layout.Children.Add(ratingsLayoutSub);

			BoxView bottomLine = new BoxView();
			bottomLine.HorizontalOptions = LayoutOptions.FillAndExpand;
			bottomLine.VerticalOptions = LayoutOptions.Start;
			bottomLine.HeightRequest = 1;
			bottomLine.BackgroundColor = Color.Silver;
			layout.Children.Add(bottomLine);

            StackLayout editorStack = new StackLayout();
            editorStack.Padding = new Thickness(20, 0, 0, 0);
            editorStack.HorizontalOptions = LayoutOptions.FillAndExpand;
            editorStack.VerticalOptions = LayoutOptions.Start;
            editorStack.IsVisible = false;

            Editor editor = new Editor();
            editor.HorizontalOptions = LayoutOptions.FillAndExpand;
            editor.VerticalOptions = LayoutOptions.Start;
            editor.HeightRequest = 100;
            editor.Text = "akjasd sfnajsf safbakf sfbkaf fjwf";

            editorStack.Children.Add(editor);
            layout.Children.Add(editorStack);


		}

		public Task<int> ReadDataFromJson()
		{
			var assembly = typeof(DynamicScreen).GetTypeInfo().Assembly;
			Stream stream = assembly.GetManifestResourceStream("SampleDB.getbannerdata.json");

			using (var reader = new System.IO.StreamReader(stream))
			{

				var json = reader.ReadToEnd();
				List<GetActiveCategoriesModel.RootObject> data = JsonConvert.DeserializeObject<List<GetActiveCategoriesModel.RootObject>>(json);

				GetActiveCategoriesModel.RootObject[] arrayobj = data.ToArray();
				for (int k = 0; k < arrayobj.Length; k++)
				{
					JSSEMasterCategory mctbl = new JSSEMasterCategory();
					mctbl.Category_ID = arrayobj[k].Category_ID;
					mctbl.Category = arrayobj[k].Category;
					App.Database.SaveCategoriesAsync(mctbl);
					for (int t = 0; t < arrayobj[k].Ratings.Count; t++)
					{
						RatingTable rtbl = new RatingTable();
						rtbl.Rating_ID = arrayobj[k].Ratings[t].Rating_ID;
						rtbl.Rating = arrayobj[k].Ratings[t].Rating;
						App.Database.SaveRatingsAsync(rtbl);

					}
					GetActiveCategoriesModel.EntBehavior[] eorgdata = arrayobj[k].EntBehaviors.ToArray();
					for (int l = 0; l < eorgdata.Length; l++)
					{
						JSSEMasterBehavior mbhtbl = new JSSEMasterBehavior();

						mbhtbl.Behavior_ID = eorgdata[l].Behavior_ID;
						mbhtbl.Behavior = eorgdata[l].Behavior;
						mbhtbl.Category_ID = arrayobj[k].Category_ID;
						mbhtbl.BehaviorType_ID = eorgdata[l].BehaviorType_ID;
						App.Database.SaveBehaviorssAsync(mbhtbl);
					}

					for (int i = 0; i < arrayobj[k].AllOrgBehaviors.Count; i++)
					{
						for (int j = 0; j < arrayobj[k].AllOrgBehaviors[i].Count; j++)
						{
							JSSEMasterBehavior mbhtbl = new JSSEMasterBehavior();

							mbhtbl.Behavior_ID = arrayobj[k].AllOrgBehaviors[i][j].Behavior_ID;
							mbhtbl.Behavior = arrayobj[k].AllOrgBehaviors[i][j].Behavior;
							mbhtbl.Category_ID = arrayobj[k].Category_ID;
							mbhtbl.Org_ID = arrayobj[k].AllOrgBehaviors[i][j].Org_ID;
							mbhtbl.BehaviorType_ID = arrayobj[k].AllOrgBehaviors[i][j].BehaviorType_ID;
							App.Database.SaveBehaviorssAsync(mbhtbl);
						}
					}
				}



			}

			return Task.FromResult(1);
		}

    }
}
