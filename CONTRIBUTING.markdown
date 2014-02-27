# How to Contribute
This is a general guide about how to contribute to Glimpse. It is not a set of hard and fast rules. Any questions, concerns or suggestions should be raised on the [Glimpse Developers List](https://groups.google.com/forum/?fromgroups#!forum/getglimpse-dev).

## Submit Bugs
Bugs should be reported in the [GitHub Issue](https://github.com/glimpse/glimpse/issues) tracker if they have not been previously submitted. 

When reporting a bug or issue, please include all pertinent information. This typically includes:

* Glimpse package(s) installed _(Example: Glimpse.Mvc3.0.87  and FluentSecurity.Glimpse.1.1)_
* Development platform, including .NET version and web server _(Example: Mvc3 with .NET version 4.0 on IIS Express)_
* If the problem is UI related include the browser and its version _(Example: IE9)_
* Steps to reproduce the bug/example code

It is also quite helpful to include the relevant portions of Glimpse's log file. You can enable Glimpse logging by adding the `<logging>` tag in web.config. Additional information can be found in [the configuration page](https://github.com/Glimpse/Glimpse/wiki/Configuration#logging).

````
	<glimpse enabled="true">
		<logging level="Trace" />
		<!-- ... -->
	</glimpse>
````

Bugs will be addressed as soon as humanly possible, but please allow ample time. For quicker responses, you may also choose to implement and contribute the bug fix. 

## Update Documentation

The [Glimpse website](http://getglimpse.com/) itself is also open source. As such, any of the documentation contained within it can be improved via contributions from the community. Feel free to submit a pull request to improve it or add documentation directly in the [wiki](https://github.com/glimpse/glimpse/wiki).

## Contribute Code

Any medium or large contribution should begin by sending a message to the [Glimpse Developers List](https://groups.google.com/forum/?fromgroups#!forum/getglimpse-dev).

The message should describe the contribution you are interested in making, and any initial thoughts on implementation. This will allow the community to discuss and become involved with you from the get go. If you receive positive feedback on the mailing list, go ahead and start implementation! You should also sign and return the [Contributor License agreement](https://www.dropbox.com/s/vxzcb7rtf1ligwh/Glimpse%20Project%20Individual%20Contributor%20License%20Agreement.pdf), which is required for the Glimpse team to accept your contribution.

The Glimpse team uses [the issue tracking features of GitHub](https://github.com/Glimpse/Glimpse/issues) which is a good place to look through if you want to get involved but aren't quite sure how.

### Code Conventions

Glimpse follows a loose set of coding conventions. Chiefly among them:

* Ensure all unit tests pass successfully
* Cover additional code with passing unit tests
* Try not to add any additional StyleCop warnings to the compilation process
* Ensure your Git autocrlf setting is true to avoid "whole file" diffs.

# Governance Model
The Glimpse project follows the meritocratic governance model. The below text describing the model is based on [the template provided at OSS Watch](http://oss-watch.ac.uk/resources/meritocraticgovernancemodel).

## Overview

This is a meritocratic, consensus-based community project. Anyone with an interest in the project can join the community, contribute to the project design and participate in the decision making process. This document describes how that participation takes place and how to set about earning merit within the project community.

## Roles and responsibilities

### Users

Users are community members who have a need for the project. They are the most important members of the community and without them the project would have no purpose. Anyone can be a user; there are no special requirements.

The project asks its users to participate in the project and community as much as possible. User contributions enable the project team to ensure that they are satisfying the needs of those users. Common user contributions include (but are not limited to):

- evangelising about the project (e.g. a link on a website and word-of-mouth awareness raising)
- informing developers of strengths and weaknesses from a new user perspective
- providing moral support (a ‘thank you’ goes a long way)
- providing financial support (the software is open source, but its developers need to eat)

Users who continue to engage with the project and its community will often become more and more involved. Such users may find themselves becoming contributors, as described in the next section.

### Contributors

Contributors are community members who contribute in concrete ways to the project. Anyone can become a contributor, and contributions can take many forms, as detailed the How to Contribute section of this document. There is no expectation of commitment to the project, no specific skill requirements and no selection process.

In addition to their actions as users, contributors may also find themselves doing one or more of the following:

- supporting new users (existing users are often the best people to support new users)
- reporting bugs
- identifying requirements
- providing graphics and web design
- programming
- assisting with project infrastructure
- writing documentation
- fixing bugs
- adding features

Contributors engage with the project through the issue tracker and mailing list, or by writing or editing documentation. They submit changes to the project itself via patches, which will be considered for inclusion in the project by existing committers (see next section). The developer mailing list is the most appropriate place to ask for help when making that first contribution.

As contributors gain experience and familiarity with the project, their profile within, and commitment to, the community will increase. At some stage, they may find themselves being nominated for committership.

### Committers

Committers are community members who have shown that they are committed to the continued development of the project through ongoing engagement with the community. Committership allows contributors to more easily carry on with their project related activities by giving them direct access to the project’s resources. That is, they can make changes directly to project outputs, without having to submit changes via patches.

This does not mean that a committer is free to do what they want. In fact, committers have no more authority over the project than contributors. While committership indicates a valued member of the community who has demonstrated a healthy respect for the project’s aims and objectives, their work continues to be reviewed by the community before acceptance in an official release. The key difference between a committer and a contributor is when this approval is sought from the community. A committer seeks approval after the contribution is made, rather than before.

Seeking approval after making a contribution is known as a commit-then-review process. It is more efficient to allow trusted people to make direct contributions, as the majority of those contributions will be accepted by the project. The project employs various communication mechanisms to ensure that all contributions are reviewed by the community as a whole. By the time a contributor is invited to become a committer, they will have become familiar with the project’s various tools as a user and then as a contributor.

Anyone can become a committer; there are no special requirements, other than to have shown a willingness and ability to participate in the project as a team player. Typically, a potential committer will need to show that they have an understanding of the project, its objectives and its strategy. They will also have provided valuable contributions to the project over a period of time.

New committers can be nominated by any existing committer. Once they have been nominated, there will be a vote by the project management committee (PMC; see below). Committer voting is one of the few activities that takes place on the project’s private management list. This is to allow PMC members to freely express their opinions about a nominee without causing embarrassment. Once the vote has been held, the aggregated voting results are published on the public mailing list. The nominee is entitled to request an explanation of any ‘no’ votes against them, regardless of the outcome of the vote. This explanation will be provided by the PMC Chair (see below) and will be anonymous and constructive in nature.

Nominees may decline their appointment as a committer. However, this is unusual, as the project does not expect any specific time or resource commitment from its community members. The intention behind the role of committer is to allow people to contribute to the project more easily, not to tie them in to the project in any formal way.

It is important to recognise that commitership is a privilege, not a right. That privilege must be earned and once earned it can be removed by the PMC (see next section) in extreme circumstances. However, under normal circumstances committership exists for as long as the committer wishes to continue engaging with the project.

A committer who shows an above-average level of contribution to the project, particularly with respect to its strategic direction and long-term health, may be nominated to become a member of the PMC. This role is described below.

### Project management committee (aka Reviewers)

The project management committee consists of those individuals identified as ‘project owners’ on the development site. The PMC has additional responsibilities over and above those of a committer. These responsibilities ensure the smooth running of the project. PMC members are expected to review code contributions, participate in strategic planning, approve changes to the governance model and manage the copyrights within the project outputs.

Members of the PMC do not have significant authority over other members of the community, although it is the PMC that votes on new committers. It also makes decisions when community consensus cannot be reached. In addition, the PMC has access to the project’s private mailing list and its archives. This list is used for sensitive issues, such as votes for new committers and legal matters that cannot be discussed in public. It is never used for project management or planning.

Membership of the PMC is by invitation from the existing PMC members. A nomination will result in discussion and then a vote by the existing PMC members. PMC membership votes are subject to consensus approval of the current PMC members.

### PMC Chair

The PMC Chair is a single individual, voted for by the PMC members. Once someone has been appointed Chair, they remain in that role until they choose to retire, or the PMC casts a two-thirds majority vote to remove them.

The PMC Chair has no additional authority over other members of the PMC: the role is one of coordinator and facilitator. The Chair is also expected to ensure that all governance processes are adhered to, and has the casting vote when the project fails to reach consensus.

## Support

All participants in the community are encouraged to provide support for new users within the project management infrastructure. This support is provided as a way of growing the community. Those seeking support should recognise that all support activity within the project is voluntary and is therefore provided as and when time allows. A user requiring guaranteed response times or results should therefore seek to purchase a support contract from a community member. However, for those willing to engage with the project on its own terms, and willing to help support other users, the community support channels are ideal.

## Contribution process

Anyone can contribute to the project, regardless of their skills, as there are many ways to contribute. For instance, a contributor might be active on the project mailing list and issue tracker, or might supply patches. The various ways of contributing are described in more detail in a separate document.

The developer mailing list is the most appropriate place for a contributor to ask for help when making their first contribution.

## Decision making process

Decisions about the future of the project are made through discussion with all members of the community, from the newest user to the most experienced PMC member. All non-sensitive project management discussion takes place on the project contributors’ mailing list. Occasionally, sensitive discussion occurs on a private list.

In order to ensure that the project is not bogged down by endless discussion and continual voting, the project operates a policy of lazy consensus. This allows the majority of decisions to be made without resorting to a formal vote.

### Lazy consensus

Decision making typically involves the following steps:

- Proposal
- Discussion
- Vote (if consensus is not reached through discussion)
- Decision

Any community member can make a proposal for consideration by the community. In order to initiate a discussion about a new idea, they should send an email to the project contributors’ list or submit a patch implementing the idea to the issue tracker (or version-control system if they have commit access). This will prompt a review and, if necessary, a discussion of the idea. The goal of this review and discussion is to gain approval for the contribution. Since most people in the project community have a shared vision, there is often little need for discussion in order to reach consensus.

In general, as long as nobody explicitly opposes a proposal or patch, it is recognised as having the support of the community. This is called lazy consensus - that is, those who have not stated their opinion explicitly have implicitly agreed to the implementation of the proposal.

Lazy consensus is a very important concept within the project. It is this process that allows a large group of people to efficiently reach consensus, as someone with no objections to a proposal need not spend time stating their position, and others need not spend time reading such mails.

For lazy consensus to be effective, it is necessary to allow at least 72 hours before assuming that there are no objections to the proposal. This requirement ensures that everyone is given enough time to read, digest and respond to the proposal. This time period is chosen so as to be as inclusive as possible of all participants, regardless of their location and time commitments.

### Voting

Not all decisions can be made using lazy consensus. Issues such as those affecting the strategic direction or legal standing of the project must gain explicit approval in the form of a vote. Every member of the community is encouraged to express their opinions in all discussion and all votes. However, only project committers and/or PMC members (as defined above) have binding votes for the purposes of decision making.

If a formal vote on a proposal is called (signaled simply by sending a email with ‘[VOTE]’ in the subject line), all participants on the project contributors’ list may express an opinion and vote. They do this by sending an email in reply to the original ‘[VOTE]’ email, with the following vote and information:

- +1 ‘yes’, ‘agree’: also willing to help bring about the proposed action
- +0 ‘yes’, ‘agree’: not willing or able to help bring about the proposed action
- -0 ‘no’, ‘disagree’: but will not oppose the action’s going forward
- -1 ‘no’, ‘disagree’: opposes the action’s going forward and must propose an alternative action to address the issue (or a justification for not addressing the issue)

To abstain from the vote, participants simply do not respond to the email. However, it can be more helpful to cast a ‘+0’ or ‘-0’ than to abstain, since this allows the team to gauge the general feeling of the community if the proposal should be controversial.

Every member of the community, from interested user to the most active developer, has a vote. The project encourages all members to express their opinions in all discussion and all votes.

However, only some members have binding votes for the purposes of decision making; for example in the Apache Software Foundation these are the committers and/or the members of the *Project Management Committee*.

It is therefore their responsibility to ensure that the opinions of all community members are considered. While not all members members may have a binding vote, a well-justified ‘-1’ from a non-committer must be considered by the community, and if appropriate, supported by a binding ‘-1’.

A ‘-1’ can also indicate a veto, depending on the type of vote and who is using it. Someone without a binding vote cannot veto a proposal, so in their case a -1 would simply indicate an objection.

When a [VOTE] receives a ‘-1’, it is the responsibility of the community as a whole to address the objection. Such discussion will continue until the objection is either rescinded, overruled (in the case of a non-binding veto) or the proposal itself is altered in order to achieve consensus (possibly by withdrawing it altogether). In the rare circumstance that consensus cannot be achieved, the PMC will decide the forward course of action.

In summary:

- Those who don’t agree with the proposal and think they have a better idea should vote -1 and defend their counter-proposal.
- Those who don’t agree but don’t have a better idea should vote -0.
- Those who agree but will not actively assist in implementing the proposal should vote +0.
- Those who agree and will actively assist in implementing the proposal should vote +1.

#### Types of approval

Different actions require different types of approval, ranging from lazy consensus to a majority decision by the PMC. These are summarised in the table below. The section after the table describes which type of approval should be used in common situations.

| Type | Description | Duration |
| ---- | ----------- | -------- |
| Lazy consensus | An action with lazy consensus is implicitly allowed, unless a binding -1 vote is received. Depending on the type of action, a vote will then be called. Note that even though a binding -1 is required to prevent the action, all community members are encouraged to cast a -1 vote with supporting argument. Committers are expected to evaluate the argument and, if necessary, support it with a binding -1. | N/A |
| Lazy majority | A lazy majority vote requires more binding +1 votes than binding -1 votes. | 72 hours |
| Consensus approval | Consensus approval requires three binding +1 votes and no binding -1 votes. | 72 hours |
| Unanimous consensus | All of the binding votes that are cast are to be +1 and there can be no binding vetoes (-1). | 120 hours |
| 2/3 majority | Some strategic actions require a 2/3 majority of PMC members; in addition, 2/3 of the binding votes cast must be +1. Such actions typically affect the foundation of the project (e.g. adopting a new codebase to replace an existing product). | 120 hours |

#### When is a vote required?

Every effort is made to allow the majority of decisions to be taken through lazy consensus. That is, simply stating one’s intentions is assumed to be enough to proceed, unless an objection is raised. However, some activities require a more formal approval process in order to ensure fully transparent decision making.

The table below describes some of the actions that will require a vote. It also identifies which type of vote should be called.

| Action | Description | Approval type |
| ------ | ----------- | ------------- |
| Release plan | Defines the timetable and actions for a release. A release plan cannot be vetoed (hence lazy majority). | Lazy majority |
| Product release | When a release of one of the project’s products is ready, a vote is required to accept the release as an official release of the project. A release cannot be vetoed (hence lazy majority). | Lazy majority | 
| New committer | A new committer has been proposed. | Consensus approval of the PMC |
| New PMC member | A new PMC member has been proposed. | Consensus approval of the community |
| Committer removal | When removal of commit privileges is sought. | Unanimous consensus of the PMC |
| PMC member removal | When removal of PMC membership is sought. | Unanimous consensus of the community |

# Additional Resources


* [General GitHub documentation](http://help.github.com/)
* [GitHub pull request documentation](http://help.github.com/send-pull-requests/)
* [Jabbr Chatroom](http://jabbr.net/#/rooms/Glimpse)
* [Nightly NuGet Feed](http://www.myget.org/F/glimpsenightly/)
* [Milestone NuGet Feed](http://www.myget.org/F/glimpsemilestone/)
* [Production NuGet Feed](https://nuget.org/api/v2/)
