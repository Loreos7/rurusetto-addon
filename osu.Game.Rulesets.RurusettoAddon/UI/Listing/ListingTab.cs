﻿using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using System.Threading.Tasks;

namespace osu.Game.Rulesets.RurusettoAddon.UI.Listing {
	public class ListingTab : OverlayTab {
		FillFlowContainer content;
		public ListingTab () {

			AddInternal( content = new FillFlowContainer {
				Direction = FillDirection.Full,
				RelativeSizeAxes = Axes.X,
				AutoSizeAxes = Axes.Y,
				Padding = new MarginPadding { Horizontal = 16, Top = 8 }
			} );
		}

		Task refreshTask = null;
		public void ReloadListing () {
			Schedule( () => {
				Overlay.StartLoading( this );
				content.Clear();
			} );

			Task task = null;
			task = refreshTask = API.RequestRulesetListing().ContinueWith( t => {
				Schedule( () => {
					if ( task != refreshTask )
						return;

					foreach ( var i in t.Result ) {
						var next = new DrawableListingEntry( i ) {
							Anchor = Anchor.TopCentre,
							Origin = Anchor.TopCentre
						};
						content.Add( next );
					}

					OnContentLoaded();
				} );
			} );
		}

		protected override bool RequiresLoading => true;
		protected override void LoadContent () {
			ReloadListing();
		}
	}
}
